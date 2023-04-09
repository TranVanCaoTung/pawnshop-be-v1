using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayNotification
    {
        public int ContractId { get; set; }
        public string ContractCode { get; set; }
        public string CustomerName { get; set; }
        public string CommodityCode { get; set; }
        public string ContractAssetName { get; set; }
        public decimal TotalPay { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string Description { get; set; }
    }
}
