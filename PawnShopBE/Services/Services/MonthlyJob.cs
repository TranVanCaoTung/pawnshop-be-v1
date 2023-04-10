using Microsoft.EntityFrameworkCore;
using Mysqlx.Resultset;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
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
    public class MonthlyJob : IJob
    {

        private readonly DbContextClass _contextClass;
        private readonly IContractService _contractService;
        private readonly IInteresDiaryService _interesDiaryService;
        private readonly ILedgerService _ledgerService;
        private readonly IBranchService _branchService;
        private readonly IRansomService _ransomService;
        private readonly IUnitOfWork _unitOfWork;

        public MonthlyJob(DbContextClass dbContextClass, IContractService contractService, IRansomService ransomService, IInteresDiaryService interesDiaryService, ILogContractService logContractService, ILedgerService ledgerService, IBranchService branchService, IUnitOfWork unitOfWork)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _ransomService = ransomService;
            _interesDiaryService = interesDiaryService;
            _ledgerService = ledgerService;
            _branchService = branchService;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var result = 0;
            var branchList = await _branchService.GetAllBranch(0);
            foreach (var branch in branchList)
            {
                // Check if old ledgers exist
                var ledger = new Ledger();
                try
                {
                    ledger = _contextClass.Ledger.FirstOrDefault(l => l.BranchId == branch.BranchId && (l.FromDate >= firstDayOfMonth) && (l.ToDate <= lastDayOfMonth));
                }
                catch (NullReferenceException e)
                {

                }
                if (ledger != null)
                {              
                    // Get Fund of month
                    ledger.Fund = ledger.Balance;

                    // Get Total Interest Money Received Of Month
                    var contractJoinInterestDiary = from contract in _contextClass.Contract
                                                    join interestDiary in _contextClass.InterestDiary
                                                    on contract.ContractId equals interestDiary.ContractId
                                                    where contract.BranchId == branch.BranchId && contract.Status == (int)ContractConst.CLOSE && (contract.ContractStartDate >= firstDayOfMonth) && (contract.ContractEndDate <= lastDayOfMonth)
                                                    select new
                                                    {
                                                        RecveivedInterest = interestDiary.PaidMoney
                                                    };
                    foreach (var row in contractJoinInterestDiary)
                    {
                        ledger.RecveivedInterest = ledger.RecveivedInterest + row.RecveivedInterest;
                    }

                    // Get Total Liquidation Money Received Of Month
                    var contractJoinLiquidation = from contract in _contextClass.Contract
                                                  join liquidation in _contextClass.Liquidtation
                                                  on contract.ContractId equals liquidation.ContractId
                                                  where contract.BranchId == branch.BranchId && contract.Status == (int)ContractConst.CLOSE && (contract.ContractStartDate >= firstDayOfMonth) && (contract.ContractEndDate <= lastDayOfMonth)
                                                  select new
                                                  {
                                                      LiquidationMoney = liquidation.LiquidationMoney
                                                  };
                    foreach (var row in contractJoinLiquidation)
                    {
                        ledger.LiquidationMoney = ledger.LiquidationMoney + row.LiquidationMoney;
                    }

                    // get total principal money received of month
                    var contractJoinRansom = from contract in _contextClass.Contract
                                             join ransom in _contextClass.Ransom
                                             on contract.ContractId equals ransom.RansomId
                                             where contract.BranchId == branch.BranchId && contract.Status == (int)ContractConst.CLOSE && (contract.ContractStartDate >= firstDayOfMonth) && (contract.ContractEndDate <= lastDayOfMonth)
                                             select new
                                             {
                                                 PrincipalMoney = ransom.Payment,
                                                 Penalty = ransom.Penalty
                                             };
                    foreach (var row in contractJoinRansom)
                    {
                        ledger.ReceivedPrincipal = ledger.ReceivedPrincipal + row.PrincipalMoney;
                        ledger.RecveivedInterest = ledger.RecveivedInterest + row.Penalty;
                    }
                    // Get Total Loan Of Month
                    var totalContractOfMonth = from contract in _contextClass.Contract
                                               where (contract.BranchId == branch.BranchId) && (contract.ContractStartDate >= firstDayOfMonth) && (contract.ContractEndDate <= lastDayOfMonth)
                                               select new
                                               {
                                                   Loan = contract.Loan
                                               };
                    foreach (var row in totalContractOfMonth)
                    {
                        ledger.Loan = ledger.Loan + row.Loan;
                    }

                    // Get Balance Of Month
                    ledger.Balance = (long)(ledger.Fund + (ledger.Loan - (ledger.ReceivedPrincipal + ledger.RecveivedInterest + ledger.LiquidationMoney)));

                    // Profit revenue if balance > fund
                    ledger.Status = (ledger.Balance > ledger.Fund) ? (int)LedgerConst.PROFIT_REVENUE : (int)LedgerConst.LOSS_REVENUE;

                    _ledgerService.UpdateLedger(ledger);
                    branch.Fund = ledger.Balance;
                    _unitOfWork.Branches.Update(branch);
                }
                else
                {
                    ledger = new Ledger();

                    ledger.FromDate = firstDayOfMonth;
                    ledger.ToDate = lastDayOfMonth;
                    ledger.BranchId = branch.BranchId;
                    ledger.RecveivedInterest = 0;
                    ledger.ReceivedPrincipal = 0;
                    ledger.LiquidationMoney = 0;
                    ledger.Loan = 0;
                    ledger.Balance = 0;
                    ledger.Fund = branch.Fund;
                    ledger.Status = (int)LedgerConst.LOSS_REVENUE;
                    ICollection<Ledger> ledgerList = new List<Ledger>();
                    _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Ledger ON;");
                    ledgerList.Add(ledger);
                    await _unitOfWork.Ledgers.AddList(ledgerList);
                    _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Ledger OFF;");
                }

            }
            await _unitOfWork.SaveList();
        }
    }
}
