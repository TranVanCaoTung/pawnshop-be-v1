using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayContractHomePage
    {
        public int BranchId { get; set; }
        public decimal Fund { get; set; }
        public int LateContract { get; set; }
        public int OpenContract { get; set;} 
        public int LiquidationContract { get; set; }
    }
}
