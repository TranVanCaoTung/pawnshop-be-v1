using PawnShopBE.Core.Data.DescriptionAttribute;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = System.Attribute;

namespace PawnShopBE.Core.DTOs
{
    public class WareHouseDTO
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseAddress { get; set; }
        public int Status { get; set; }

        public ICollection<ContractAssetDTO>? ContractAssets { get; set; }
    }
}
