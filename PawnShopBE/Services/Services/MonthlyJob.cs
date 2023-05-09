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
        private readonly ILiquidationService _liquidationService;
        private readonly IUnitOfWork _unitOfWork;

        public MonthlyJob(DbContextClass dbContextClass, IContractService contractService, IRansomService ransomService, IInteresDiaryService interesDiaryService, ILogContractService logContractService, ILedgerService ledgerService, IBranchService branchService, ILiquidationService liquidationService, IUnitOfWork unitOfWork)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _ransomService = ransomService;
            _interesDiaryService = interesDiaryService;
            _ledgerService = ledgerService;
            _branchService = branchService;
            _liquidationService = liquidationService;
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
                    decimal totalInterestGet = 0;
                    decimal totalRansom = 0;
                    decimal totalLiquidation = 0;
                    ledger.Revenue = 0;
                    ledger.Loan = 0;
                    ledger.Profit = 0;
                    var contractsOfBranch = await _contextClass.Set<Contract>()
                                         .Where(c => c.BranchId == branch.BranchId)
                                         .ToListAsync();
                    foreach (var contract in contractsOfBranch)
                    {
                        var interestDiaryOfMonth = await _interesDiaryService.GetInteresDiariesByContractId(contract.ContractId);
                        foreach (var interestDiary in interestDiaryOfMonth)
                        {
                            // Get interest money paid each day
                            if (interestDiary != null)
                            {
                                totalInterestGet += interestDiary.PaidMoney;
                            }
                        }
                        // Get money ransom paid each day
                        var ransomOfMonth = await _ransomService.GetRansomByContractId(contract.ContractId);
                        if (ransomOfMonth != null)
                        {
                            totalRansom += ransomOfMonth.PaidMoney;
                        }

                        // Get money liquidation paid each day
                        var liquidationOfMonth = await _liquidationService.GetLiquidationById(contract.ContractId);
                        if (liquidationOfMonth != null)
                        {
                            totalLiquidation += liquidationOfMonth.LiquidationMoney;
                        }
                        ledger.Revenue = totalLiquidation + totalRansom + totalInterestGet;
                        ledger.Loan = contract.Loan;
                        ledger.Profit = ledger.Revenue - ledger.Loan;
                        _ledgerService.UpdateLedger(ledger);
                    }
                }
                else
                {
                    ledger = new Ledger();

                    ledger.FromDate = firstDayOfMonth;
                    ledger.ToDate = lastDayOfMonth;
                    ledger.BranchId = branch.BranchId;
                    ledger.Revenue = 0;
                    ledger.Profit = 0;
                    ledger.Loan = 0;
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
