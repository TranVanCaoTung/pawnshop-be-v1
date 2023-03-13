using PawnShopBE.Core.Const;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Quartz;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ScheduleJob : IJob
    {
        private readonly DbContextClass _contextClass;
        private readonly IContractService _contractService;
        private readonly IPackageService _packageService;
        private readonly IInteresDiaryService _interesDiaryService;
        public ScheduleJob(DbContextClass dbContextClass, IContractService contractService, IPackageService packageService, IInteresDiaryService interesDiaryService)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _packageService = packageService;
            _interesDiaryService = interesDiaryService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var overdueContracts = _contextClass.Contract
                        .Where(c => c.Status == (int)ContractConst.IN_PROGRESS && c.ContractEndDate < DateTime.Today)
                        .ToList();

            var overdueDiaries = _contextClass.InterestDiary
                        .Where(d => d.Status == (int) InterestDiaryConsts.NOT_PAID && d.NextDueDate < DateTime.Today)
                        .ToList();

            foreach (var diary in overdueDiaries)
            {
                // Payment Fee for Interest if overdue periods
                if (diary.Penalty == 0)
                {
                    diary.Penalty = diary.Payment / 2;
                }
                diary.TotalPay = diary.Penalty + diary.Payment;

                var contract = await _contractService.GetContractById(diary.ContractId);
                var package = await _packageService.GetPackageById(contract.PackageId, contract.InterestRecommend);


                // Payment Fee for Interest if overdue the last period
                if (diary.NextDueDate == contract.ContractEndDate)
                { 
                    // Calculate how many days that overdue
                    TimeSpan timeDifference = DateTime.Now -contract.ContractEndDate;
                    double totalDays = timeDifference.TotalDays;

                    decimal paymentFee = (contract.Loan + (contract.Loan * package.PackageInterest)) * (1 + package.PackageInterest);
                    
                    // Penalty for < 1 month and > 3 month
                    if (package.PunishDay2 != 0)
                    {
                        // Overdue punish day 1
                        if (totalDays == (double) package.PunishDay1 )
                        {
                            diary.Penalty = paymentFee;
                        } 
                        // Overdue punish day 2
                        else if (totalDays == package.PunishDay2)
                        {
                            diary.Penalty = paymentFee * (1 + package.PackageInterest);
                        } 
                        // Over liquidation day
                        else if (totalDays == package.LiquitationDay)
                        {
                            contract.Status = (int) ContractConst.LIQUIDATION;
                        }
                    }
                    // Penalty between 1 month to 3 month
                    else
                    {
                        if (totalDays == (double)package.PunishDay1)
                        {
                            diary.Penalty = paymentFee;
                        }                     
                        // Over liquidation day
                        else if (totalDays == package.LiquitationDay)
                        {
                            contract.Status = (int)ContractConst.LIQUIDATION;
                        }
                    }
                }
            }



            foreach (var contract in overdueContracts)
            {
                contract.Status = (int)ContractConst.OVER_DUE;
            }

            _contextClass.SaveChanges();
        }
    }
}
