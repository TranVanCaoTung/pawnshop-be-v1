using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayReportTransaction
    {
        public int ContractId { get; set; }
        public string ContractCode { get; set; }
        public string CustomerName { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public decimal Loan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
