using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayContractInfo
    {
        public int ContractId { get; set; }
        public int WarehouseId { get; set; }
        public int ContractAssetId { get; set; }
        //Customer info
        public string CustomerName { get; set; }
        public string CCCD { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //Pawn info
        public string ContractCode { get; set; }
        public string TypeOfProduct { get; set; }
        public string AssetName { get; set; }
        public decimal InsuranceFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal Loan { get; set; }
        public string UserName { get; set; }
        //Asset info
        public ICollection<string> AttributeInfos { get; set; }
        public string AssetImg { get; set; }
        //Package info
        public string PackageName { get; set; }
        public int PaymentPeriod { get; set; }
        public DateTime ContractStartDate { get; set; }
        public int PackageInterest { get; set; }
        public int InterestRecommend { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalRecived { get; set; }
        public string WarehouseName { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal InterestDebt { get; set; }
        public int Status { get; set; }
    }
}
