using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayCustomer
    {
        public Guid customerId { get; set; }
        public string nameBranch { get; set; }
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Point { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Reason { get; set; }
        public int Status { get; set; }
    }
}
