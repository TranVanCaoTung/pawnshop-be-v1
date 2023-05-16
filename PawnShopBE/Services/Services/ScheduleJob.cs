using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Data;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Quartz;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Services
{
    public class ScheduleJob : IJob
    {
        private readonly DbContextClass _contextClass;
        private readonly IContractService _contractService;
        private readonly IPackageService _packageService;
        private readonly IInteresDiaryService _interesDiaryService;
        private readonly ILogContractService _logContractService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleJob(DbContextClass dbContextClass, IContractService contractService, IPackageService packageService, IInteresDiaryService interesDiaryService, ILogContractService logContractService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _packageService = packageService;
            _interesDiaryService = interesDiaryService;
            _logContractService = logContractService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Contracts IN_PROGRESS turn into OVER_DUE 
            var overdueContracts = _contextClass.Contract
                        .Where(c => c.Status == (int)ContractConst.IN_PROGRESS && c.ContractEndDate < DateTime.Today)
                        .ToList();
            foreach (var contract in overdueContracts)
            {
                contract.Status = (int)ContractConst.OVER_DUE;
            }

            // Ransom on time
            var ramsomsOnTime = _contextClass.Ransom
                        .Where(r => r.Status == (int)RansomConsts.SOON && r.Contract.ContractEndDate == DateTime.Today)
                        .ToList();
            foreach (var ransom in ramsomsOnTime)
            {
                ransom.TotalPay = ransom.TotalPay - ransom.Penalty;
                ransom.Penalty = 0;
                ransom.Status = (int)RansomConsts.ON_TIME;
            }

            // Ransom overdue date
            var ransomOverDueDate = _contextClass.Ransom
                        .Where(r => r.PaidDate == null && r.Contract.ContractEndDate < DateTime.Today && r.Status != (int)RansomConsts.LATE)
                        .ToList();

            foreach (var ransom in ransomOverDueDate)
            {
                var contract = await _contractService.GetContractById(ransom.ContractId);
                var package = await _packageService.GetPackageById(contract.PackageId);

                // Calculate how many days that overdue
                TimeSpan timeDifference = DateTime.Now - contract.ContractEndDate;
                double totalDays = timeDifference.TotalDays;

                decimal paymentFee = (contract.Loan * (1 + package.InterestDiaryPenalty)) * package.InterestDiaryPenalty;

                // Penalty for Punish Day 2 != 0
                if (package.PunishDay2 != 0)
                {
                    // Overdue punish day 1
                    if (totalDays == (double)package.PunishDay1 || totalDays < (double)package.PunishDay2)
                    {
                        ransom.Penalty = paymentFee;
                    }
                    // Overdue punish day 2
                    if (totalDays == (double)package.PunishDay2 || totalDays < (double)package.LiquitationDay)
                    {
                        ransom.Penalty = paymentFee * 2;
                    }
                }
                // Penalty for Punish Day 2 == 0
                else
                {
                    if (totalDays == (double)package.PunishDay1 || totalDays < (double)package.LiquitationDay)
                    {
                        ransom.Penalty = paymentFee;
                    }
                }
                // Over liquidation day                
                if (totalDays == package.LiquitationDay || totalDays > (double)package.LiquitationDay)
                {
                    contract.Status = (int)ContractConst.LIQUIDATION;
                }
                ransom.Status = (int)RansomConsts.LATE;
            }
            var overdueDiaries = _contextClass.InterestDiary
                        .Where(d => d.Status == (int)InterestDiaryConsts.NOT_PAID && d.NextDueDate < DateTime.Today && d.Penalty == 0)
                        .ToList();

            foreach (var diary in overdueDiaries)
            {
                var existContract = await _contractService.GetContractById(diary.ContractId);
                var package = await _packageService.GetPackageById(existContract.PackageId);

                // Payment Fee for Interest if overdue periods
                if (diary.Penalty == 0)
                {
                    diary.Penalty = diary.Payment * (package.InterestDiaryPenalty * (decimal)0.01);
                }
                diary.TotalPay = diary.Penalty + diary.Payment;

                // Log Contract when overdueDate
                var contractJoinUserJoinCustomer = from contract in _contextClass.Contract
                                                   join customer in _contextClass.Customer
                                                   on contract.CustomerId equals customer.CustomerId
                                                   join user in _contextClass.User
                                                   on contract.UserId equals user.UserId
                                                   select new
                                                   {
                                                       ContractId = contract.ContractId,
                                                       UserName = user.FullName,
                                                       CustomerName = customer.FullName,
                                                   };
                var logContract = new LogContract();
                foreach (var row in contractJoinUserJoinCustomer)
                {
                    logContract.ContractId = row.ContractId;
                    logContract.UserName = row.UserName;
                    logContract.CustomerName = row.CustomerName;
                }
                logContract.Debt = diary.TotalPay;
                logContract.Paid = 0;
                logContract.Description = "Tiền lãi kỳ " + diary.NextDueDate.ToString("dd/MM/yyyy") + " chưa thanh toán số tiền " + logContract.Debt.ToString() + " VND.";
                logContract.EventType = (int)LogContractConst.INTEREST_NOT_PAID;
                logContract.LogTime = DateTime.Now;
                await _logContractService.CreateLogContract(logContract);
            }

            // Scan for customer point < 0 and turn status to BLACKLIST
            var customerList = await _contextClass.Customer.Where(x => x.Status == (int)CustomerConst.ACTIVE && x.Point < 0).ToListAsync();
            foreach(var  customer in customerList)
            {
                customer.Status = (int)CustomerConst.BLACKLIST;
                customer.Reason = "Trễ hạn thanh toán nhiều hợp đồng.";
                _contextClass.Customer.Update(customer);
            }
            // Create notification for contract end date.
            var contractsListToday = await _contextClass.Contract.Where(x => x.ContractEndDate.Date == DateTime.Now.Date).ToListAsync();
            // Get notfication have been created today
            var notificationListToday = await _contextClass.Notifications.Where(x => x.CreatedDate.Date == DateTime.Now.Date).ToListAsync();
            if (notificationListToday.Count != contractsListToday.Count)
            {
                var notificationList = new List<Notification>();
                foreach (var contract in contractsListToday)
                { 
                    var notifi = new Notification();
                    notifi.BranchId = contract.BranchId;
                    notifi.Header = "Hợp đồng đến hạn";
                    notifi.Content = "Hợp đồng " + contract.ContractCode + " đã đến hạn cần thanh toán.";
                    notifi.Type = (int)NotificationConst.CONTRACT_END_DATE;
                    notifi.CreatedDate = DateTime.Now;
                    notifi.IsRead = false;
                    notificationList.Add(notifi);
                }
                _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Notification ON;");
                await _unitOfWork.Notifications.AddList(notificationList);
                _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary OFF;");
            }           
            _contextClass.SaveChanges();
        }
    }
}
