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
        [Required(ErrorMessage =  "Tên kho không được để trống")]
        [StringLength(30,MinimumLength =6,ErrorMessage ="Tên kho phải dài từ 6 - 30 ký tự")]
        [Description("Tên Kho")]
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Địa chỉ kho không được để trống")]
        public string WarehouseAddress { get; set; }
        public int Status { get; set; }

        public ICollection<ContractAssetDTO>? ContractAssets { get; set; }
    }
}
