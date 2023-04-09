using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        public int Status { get; set; }
    }  
}
