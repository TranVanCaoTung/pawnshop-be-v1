using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Data
{
    public class Login
    {
        public Guid? UserId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }
    }
}
