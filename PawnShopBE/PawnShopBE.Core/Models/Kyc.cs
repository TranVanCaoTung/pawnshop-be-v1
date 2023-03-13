using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Kyc
    {
        public int KycId { get; set; }
        public string? IdentityCardFronting { get; set; }
        public string? IdentityCardBacking { get; set; }
        public string? FaceImg { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
