using AutoMapper;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private IContractService _contract;
        private ICustomerService _customer;
        private IContractAssetService _asset;
        private IPawnableProductService _pawnable;
        private ILedgerService _ledgerService;
        private ILiquidationService _liquidation;
        private IBranchService _branch;
        private DbContextClass _dbContextClass;
        private IMapper _mapper;

        public ReportService(IContractService contract, ICustomerService customer,
            IContractAssetService asset, IPawnableProductService pawnable, ILedgerService ledger,
            ILiquidationService liquidation, IBranchService branch,
            DbContextClass dbContextClass, IMapper mapper)
        {
            _contract = contract;
            _customer = customer;
            _asset = asset;
            _pawnable = pawnable;
            _ledgerService = ledger;
            _liquidation = liquidation;
            _branch = branch;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DisplayReportTransaction>> getReportTransaction(int number)
        {
            var reportList = new List<DisplayReportTransaction>();
            //get customer
            var customerList = await _customer.GetAllCustomer(0);
            //get Asset
            var assetList = await _asset.GetAllContractAssets();
            //get pawnable
            var pawnableList = await _pawnable.GetAllPawnableProducts(0);
            //get contract
            var getAllContract = await _contract.GetAllContracts(0);
            var contractList = getAllContract.Where(c => c.Status == 4).ToList();
            foreach (var contract in contractList)
            {
                var customer = getCustomer(contract.CustomerId, customerList);
                var asset = getAsset(contract.ContractAssetId, assetList);
                var pawnable = getPawnable(asset.PawnableProductId, pawnableList);
                //đưa value vào report
                var report = new DisplayReportTransaction();
                report.ContractCode = contract.ContractCode;
                report.CustomerName = customer.FullName;
                report.AssetCode = pawnable.CommodityCode;
                report.AssetName = asset.ContractAssetName;
                report.Loan = contract.Loan;
                report.StartDate = contract.ContractStartDate;
                report.EndDate = contract.ContractEndDate;
                reportList.Add(report);
            }

            if (number == 0)
            {
                return reportList;
            }
            var result = await TakePage(number, reportList);
            return result;

        }
        private async Task<IEnumerable<DisplayReportTransaction>> TakePage(int number, IEnumerable<DisplayReportTransaction> list)
        {
            var numPage = (int)NumberPage.numPage;
            var skip = (numPage * number) - numPage;
            return list.Skip(skip).Take(numPage);
        }

        private PawnableProduct getPawnable(int pawnableProductId, IEnumerable<PawnableProduct> pawnableList)
        {
            var pawnable = (from p in pawnableList where p.PawnableProductId == pawnableProductId select p).FirstOrDefault();
            return pawnable;
        }

        private ContractAsset getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList)
        {
            var asset = (from a in assetList where a.ContractAssetId == contractAssetId select a).FirstOrDefault();
            return asset;
        }

        private Customer getCustomer(Guid customerId, IEnumerable<Customer> customerList)
        {
            var customer = (from c in customerList where c.CustomerId == customerId select c).FirstOrDefault();
            return customer;
        }

        public async Task<List<DisplayReportMonth>> getReportMonth(int branchId)
        {
            var ledgerList = await _ledgerService.GetLedgersByBranchId(branchId);
            var newReportList = new List<DisplayReportMonth>(); 
            foreach (var ledger in ledgerList)
            {
                var report = new DisplayReportMonth();
                report.BranchId = ledger.BranchId;
                report.Month = ledger.ToDate.Month;
                report.Year = ledger.ToDate.Year;
                report.Fund = ledger.Fund;
                report.Loan = ledger.Loan;
                report.ReceivedPrincipal = ledger.ReceivedPrincipal;
                report.ReceiveInterest = ledger.RecveivedInterest;
                report.Balance = ledger.Balance;
                report.LiquidationMoney = ledger.LiquidationMoney;
                report.Status = ledger.Status;
                newReportList.Add(report);
            }


            return newReportList;
        }

        //private decimal getLiquiMoney(int branchId, IEnumerable<Contract> contractList,
        //    IEnumerable<Liquidtation> liquidationList, int month)
        //{
        //    //get contract
        //    var contractIenumerable = from c in contractList where c.ActualEndDate?.Month == month select c;
        //    decimal money = 0;
        //    foreach (var contract in contractIenumerable)
        //    {
        //        //get liquidation
        //        var liquidation = (from l in liquidationList where l.ContractId == contract.ContractId select l).FirstOrDefault();
        //        if (liquidation != null)
        //            money += liquidation.LiquidationMoney;
        //    }
        //    return money;
        //}

        //private Branch getBranch(IEnumerable<Branch> branchList, IEnumerable<Ledger> ledgerMonth)
        //{
        //    var branch = (branchList.Join(ledgerMonth, b => b.BranchId, l => l.BranchId,
        //        (b, l) => { return b; })).FirstOrDefault();
        //    return branch;
        //}

        private IEnumerable<Ledger> getLedgerMonth(IEnumerable<Ledger> ledgerList, int month)
        {
            var list = from l in ledgerList where l.ToDate.Month == month select l;
            return list;
        }



    }
}
