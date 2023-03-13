using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Liquidtation
    {
        public int LiquidationId { get; set; }
        public int ContractId { get; set; }
        public int LiquidationMoney { get; set; }
        public int liquidationDate { get; set; }
        public int Description { get; set; }


        public virtual Contract? Contract { get; set; }

    }
}
