using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Mysqlx.Crud;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Quartz;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
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
        public ScheduleJob(DbContextClass dbContextClass, IContractService contractService, IPackageService packageService, IInteresDiaryService interesDiaryService, ILogContractService logContractService)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _packageService = packageService;
            _interesDiaryService = interesDiaryService;
            _logContractService = logContractService;
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

                decimal paymentFee = (contract.Loan + (contract.Loan * package.PackageInterest)) * (1 + package.PackageInterest);

                // Penalty for < 1 month and > 3 month
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
                        ransom.Penalty = paymentFee * (1 + package.PackageInterest);
                    }
                }
                // Penalty between 1 month to 3 month
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
                // Payment Fee for Interest if overdue periods
                if (diary.Penalty == 0)
                {
                    diary.Penalty = diary.Payment / 2;
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
                logContract.Description = diary.NextDueDate.ToString("MM/dd/yyyy HH:mm");
                logContract.EventType = (int)LogContractConst.INTEREST_NOT_PAID;
                logContract.LogTime = DateTime.Now;
                await _logContractService.CreateLogContract(logContract);
            }
            _contextClass.SaveChanges();
        }
    }
}
