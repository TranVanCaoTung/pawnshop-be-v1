using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayBranchDetail
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal Loan { get; set; }
        public decimal CurrentFund { get; set; }
        public decimal Profit { get;set; }
        public int TotalContracts { get; set; }
        public int OpenContract { get; set; }
        public int CloseContract { get; set; }
    }
}
