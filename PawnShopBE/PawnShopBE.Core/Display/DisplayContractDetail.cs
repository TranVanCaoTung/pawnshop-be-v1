using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayContractDetail
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public decimal Loan { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public int PackageInterest { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal InterestDebt { get; set; }
        public int Status { get; set; }

    }
}
