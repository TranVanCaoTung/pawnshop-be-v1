using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class UserBranch
    {
        public Guid UserId { get; set; }
        public int BranchId { get; set; }

        public virtual User? User { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
