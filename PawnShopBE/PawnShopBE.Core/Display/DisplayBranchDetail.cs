using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayBranchDetail
    {
        public int branchId { get; set; }
        public string branchName { get; set; }
        public decimal balance { get; set; }
        public decimal fund { get; set; }
        public decimal loanLedger { get; set; }
        public decimal loanContract { get; set; }
        public decimal totalProfit{ get; set; }
        public decimal recveivedInterest { get; set; }
        public decimal debtCustomers { get; set; }
        public int numberContract { get; set; }
        public int openContract { get; set; }
        public int closeContract { get; set; }
    }
}
