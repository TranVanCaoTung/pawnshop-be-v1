using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class ContractAssetDTO
    {
        public int? WarehouseId { get; set; }
        public int PawnableProductId { get; set; }

        [Required(ErrorMessage = "Tên tài sản không được để trống")]
        [StringLength(30, MinimumLength = 6, 
            ErrorMessage = "Tên tài sản phải dài từ 6 - 30 ký tự")]
        public string ContractAssetName { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cần có hình ảnh xác thực")]
        public string Image { get; set; }
        public string? commodifyCode { get; set; }

    }
}
