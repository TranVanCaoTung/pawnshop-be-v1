using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseAddress { get; set; }
        public int Status { get; set; }
        public ICollection<ContractAsset> ContractAssets { get; set; }

        public Warehouse()
        {
            ContractAssets = new List<ContractAsset>();
        }
    }
}
