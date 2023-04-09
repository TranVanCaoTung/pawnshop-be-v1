using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class MoneyDTO
    {
        public int MoneyId { get; set; }
        public int BranchId { get; set; }
        public DateTime DateCreate { get; set; }
        public string UserName { get; set; }
        public decimal MoneyInput { get; set; }
        public int Status { get; set; }
    }
}
