using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayLiquidationDetail
    {
        public string TypeOfProduct { get; set; }
        public string AssetName { get; set; }
        public decimal LiquidationMoney { get; set; }
        public DateTime LiquidationDate { get; set; }
        public string Description { get; set; }
    }
}
