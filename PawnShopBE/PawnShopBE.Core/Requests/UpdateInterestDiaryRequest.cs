using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Requests
{
    public class UpdateInterestDiaryRequest
    {
        public decimal PaidMoney { get; set; }
        public List<string> ProofImg { get; set; }
    }
}
