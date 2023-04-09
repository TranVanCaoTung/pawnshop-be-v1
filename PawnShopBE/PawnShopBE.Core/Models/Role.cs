using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PawnShopBE.Core.Models
{
    public class Role//:IdentityRole<Guid>
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<User>? Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
