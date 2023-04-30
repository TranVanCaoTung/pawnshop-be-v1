using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayBranch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal CurrentFund { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int LiquidationContracts { get; set; }
    }
}
