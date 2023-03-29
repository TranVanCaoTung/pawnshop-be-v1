using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayBranch
    {
        public int branchId { get; set; }
        public string branchName { get; set; }
        public decimal balance { get; set; }
        public decimal fund { get; set; }
        public decimal loan { get; set; }
        public decimal recveivedInterest { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
    }
}
