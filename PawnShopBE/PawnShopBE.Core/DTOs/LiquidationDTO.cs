using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class LiquidationDTO
    {
        public int ContractId { get; set; }
        public int LiquidationMoney { get; set; }
        public int liquidationDate { get; set; }
        public int Description { get; set; }
    }
}
