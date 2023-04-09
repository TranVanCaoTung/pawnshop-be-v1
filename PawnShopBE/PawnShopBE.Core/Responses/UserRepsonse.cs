using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Responses
{
    public class UserRepsonse
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
