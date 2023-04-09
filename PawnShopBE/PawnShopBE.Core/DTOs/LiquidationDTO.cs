using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class LiquidationDTO
    {
        public int LiquidationId { get; set; }
        public int ContractId { get; set; }
        public decimal LiquidationMoney { get; set; }
        public DateTime liquidationDate { get; set; }
        public string? Description { get; set; }
    }
}
