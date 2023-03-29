using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    
    public class UserPermissionGroup
    {
        public Guid UserId { get; set; }

        public int perId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("perId")]
        public virtual Permission Permission { get; set; }
        public bool Status { get; set; }

    }
}
