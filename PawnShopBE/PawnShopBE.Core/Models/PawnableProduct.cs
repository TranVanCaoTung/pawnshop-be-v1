using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class PawnableProduct
    {
        public int PawnableProductId { get; set; }
        public string TypeOfProduct { get; set; }
        public string CommodityCode { get; set; }
        public int Status { get; set; }

        public ICollection<Attribute>? Attributes { get; set; }
        public ICollection<ContractAsset>? ContractAssets { get; set; }

        public PawnableProduct()
        {
            ContractAssets = new List<ContractAsset>();
            Attributes = new List<Attribute>();
        }
    }
}
