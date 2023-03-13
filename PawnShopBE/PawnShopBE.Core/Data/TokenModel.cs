using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Data
{
   public class TokenModel
    {
        public string AccessToken { get; set; }
        public string? RefeshToken { get; set; }
    }
}
