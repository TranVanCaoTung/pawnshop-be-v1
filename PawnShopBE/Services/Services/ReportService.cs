using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private ILedgerService _ledger;
        private ILiquidationService _liquidation;
        private IBranchService _branch;

        public ReportService(IContractService contract, ICustomerService customer, 
            IContractAssetService asset, IPawnableProductService pawnable, ILedgerService ledger,
            ILiquidationService liquidation, IBranchService branch)
        {
            _contract = contract;
            _customer = customer;
            _asset = asset;
            _pawnable = pawnable;
            _ledger = ledger;
            _liquidation = liquidation;
            _branch = branch;
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
            var contractList = await _contract.GetAllContracts(0);

            foreach (var contract in contractList)
            {
                var customer = getCustomer(contract.CustomerId, customerList);
                var asset= getAsset(contract.ContractAssetId,assetList);
                var pawnable = getPawnable(asset.PawnableProductId, pawnableList);
                //đưa value vào report
                var report = new DisplayReportTransaction();
                report.contractCode= contract.ContractCode;
                report.customerName = customer.FullName;
                report.assetCode = pawnable.CommodityCode;
                report.assetName = asset.ContractAssetName;
                report.loan=contract.Loan;
                report.startDate = contract.ContractStartDate; 
                report.endDate=contract.ContractEndDate;
                reportList.Add(report);
            }

            if (number == 0)
            {
                return reportList;
            }
            var result =await TakePage(number, reportList);
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
            var pawnableIenumerable= from p in pawnableList where p.PawnableProductId == pawnableProductId select p;
            var pawnable=pawnableIenumerable.FirstOrDefault();
            return pawnable;
        }

        private ContractAsset getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList)
        {
            var assetIenumerable= from a in assetList where a.ContractAssetId== contractAssetId select a;
            var asset= assetIenumerable.FirstOrDefault();
            return asset;
        }

        private Customer getCustomer(Guid customerId, IEnumerable<Customer> customerList)
        {
            var customerIenumerable = from c in customerList where c.CustomerId == customerId select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer;
        }
       
        public async Task<IEnumerable<DisplayReportMonth>> getReportMonth()
        {
            var listReport= new List<DisplayReportMonth>();
            //get Ledger
            var ledgerList = await _ledger.GetLedger();
            //get branch 
            var branchList = await _branch.GetAllBranch(0);
            //get liquidation
            var liquidationList= await _liquidation.GetLiquidation();
            //get contract
            var contractList = await _contract.GetAllContracts(0);
            int month = 1;
            for (int i = 0; i < 12; i++)
            {
                //get ledger theo 12 tháng
                var ledgerMonth = getLedgerMonth(ledgerList, month);
                var x=ledgerMonth.ToList();
                if (x.Count==0)
                {
                    month++;
                }
                else
                {
                    var report = new DisplayReportMonth();
                    foreach (var ledger in ledgerMonth)
                    {
                        report.receiveInterest = ledger.RecveivedInterest;
                        report.receivedPrincipal = ledger.ReceivedPrincipal;
                        report.loan = ledger.Loan;
                        report.balance = ledger.Balance;
                    }
                    //ger branch
                    var branch = getBranch(branchList, ledgerMonth);
                    report.fund = branch.Fund;
                    //get money
                    report.liquidationMoney = getLiquiMoney(branch.BranchId, contractList, liquidationList, month);
                    report.month = month;
                    //add report
                    listReport.Add(report);
                    month++;
                }
            }
            return listReport;
        }

        private decimal getLiquiMoney(int branchId, IEnumerable<Contract> contractList,
            IEnumerable<Liquidtation> liquidationList, int month)
        {
            //get contract
            var contractIenumerable = from c in contractList where c.ActualEndDate?.Month == month select c;
            decimal money = 0;
            foreach (var contract in contractIenumerable)
            {
                //get liquidation
                var liquidationIenumerable = from l in liquidationList where l.ContractId == contract.ContractId select l;
                var liquidation= liquidationIenumerable.FirstOrDefault();
                money += liquidation.LiquidationMoney;
            }
            return money;
        }

        private Branch getBranch(IEnumerable<Branch> branchList, IEnumerable<Ledger> ledgerMonth)
        {
            var branchIenumerable = branchList.Join(ledgerMonth, b => b.BranchId, l => l.BranchId,
                (b, l) => { return b; });
            var branch=branchIenumerable.FirstOrDefault();
            return branch;
        }

        private IEnumerable<Ledger> getLedgerMonth(IEnumerable<Ledger> ledgerList, int month)
        {
            var list = from l in ledgerList where l.ToDate.Month == month select l;
            return list;
        }



    }
}
