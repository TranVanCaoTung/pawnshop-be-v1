using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayReportTransaction
    {
       public string contractCode { get; set; }
        public string customerName { get; set; }
        public string assetCode { get; set; }
        public string assetName { get; set; }
        public decimal loan { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}
