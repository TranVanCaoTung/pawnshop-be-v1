using PawnShopBE.Core.Data.DescriptionAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Warehouse
    {
        [Description("Id Kho")]
        public int WarehouseId { get; set; }
        [Description("Tên Kho")]
        public string WarehouseName { get; set; }
        [Description("Tên Địa Chỉ")]
        public string WarehouseAddress { get; set; }
        [Description("Trạng Thái Hoạt Động Kho")]
        public int Status { get; set; }
        public ICollection<ContractAsset> ContractAssets { get; set; }

        public Warehouse()
        {
            ContractAssets = new List<ContractAsset>();
        }
    }
}
