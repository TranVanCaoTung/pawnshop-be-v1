using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayReportMonth
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Fund { get; set; }
        public decimal Loan { get; set; }
        // tiền lãi đã nhận
        public decimal ReceiveInterest { get; set; }
        //(tiền gốc đã nhận)
        public decimal ReceivedPrincipal { get; set; }
        //số dư
        public decimal Balance { get; set; }
        //tiền thanh lý
        public decimal LiquidationMoney { get; set; }
        public int Status { get; set; }
    }
}
