using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayReportMonth
    {
        public int month { get; set; }
        public decimal fund { get; set; }
        public decimal loan { get; set; }
        // tiền lãi đã nhận
        public decimal receiveInterest { get; set; }
        //(tiền gốc đã nhận)
        public decimal receivedPrincipal { get; set; }
        //số dư
        public decimal balance { get; set; }
        //tiền thanh lý
        public decimal liquidationMoney { get; set; }
    }
}
