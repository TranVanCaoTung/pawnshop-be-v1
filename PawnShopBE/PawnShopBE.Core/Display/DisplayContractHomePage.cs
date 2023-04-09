using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayContractHomePage
    {
        //public string contractCode { get; set; }
        //public string customerName { get; set; }
        //public string assestCode { get; set; }
        //public string assetName { get; set; }
        //public decimal loanContract { get; set; }
        //public DateTime startDate { get; set; }
        //public DateTime endDate { get; set; }
        //public string wareName { get; set; }
        //public int status { get; set; }
        //field hiện chung cộng dồn
        public decimal? fund { get; set; }= 0;
        public decimal? loanLedger { get; set; } = 0;
        public decimal? recveivedInterest { get; set; } = 0;
        public decimal? totalProfit { get; set; } = 0;
        public decimal? ransomTotal { get; set;} = 0;
        public decimal? openContract { get; set;} = 0;

    }
}
