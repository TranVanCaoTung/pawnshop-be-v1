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
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Status { get; set; }
        public ICollection<DisplayUserBranch> UserBranches { get; set; }
        public ICollection<DisplayPermission> UserPermission { get; set; }
        public DisplayUser()
        {
            UserBranches = new List<DisplayUserBranch>();
            UserPermission = new List<DisplayPermission>();
        }
    }
}
