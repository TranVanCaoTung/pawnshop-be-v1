using PawnShopBE.Core.Models;
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
        public ICollection<int> BranchIds { get; set; }      
    }
}
