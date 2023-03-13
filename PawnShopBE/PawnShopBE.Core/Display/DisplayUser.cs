using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayUser
    {
        public int Num { get; set; }
        public Guid UserId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
    }
}
