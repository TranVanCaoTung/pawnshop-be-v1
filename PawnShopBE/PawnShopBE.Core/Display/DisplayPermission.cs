using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayPermission
    {
        public Guid UserId { get; set; }    
        public int PermissionId { get; set; }
        public string NamePermission { get; set; }
        public bool Status { get; set; }
    }
}
