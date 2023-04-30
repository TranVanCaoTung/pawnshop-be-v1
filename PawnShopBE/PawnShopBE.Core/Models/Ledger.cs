using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Ledger
    {
        public int LedgerId { get; set; }
        public int BranchId { get; set; }
        public decimal Loan { get; set; }
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public virtual Branch Branch { get; set; }
    }
}
