using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Fund { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }

        //relationship
        public ICollection<Contract>? Contracts { get; set; }
        public ICollection<Ledger>? Ledgers { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserBranch> UserBranches { get; set; }
        public Branch()
        {
            Contracts = new List<Contract>();
            Ledgers = new List<Ledger>();
            Notifications = new List<Notification>();
            UserBranches = new List<UserBranch>();
        }
    }
}
