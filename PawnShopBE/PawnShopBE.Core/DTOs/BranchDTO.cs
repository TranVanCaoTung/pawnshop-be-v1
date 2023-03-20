using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class BranchDTO
    {
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [StringLength(30, MinimumLength = 6, 
           ErrorMessage = "Tên chi nhánh phải dài từ 6 - 30 ký tự")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Địa chỉ chi nhánh không được để trống")]
        public string Address { get; set; }

        [Phone]
        [StringLength(11, MinimumLength = 10,
            ErrorMessage = "Số điện thoại phải từ 10-11 số")]
        public string PhoneNumber { get; set; }

        [Range(1000,10000000000,ErrorMessage ="Tiền nhập phải từ 1000 - 10 Tỷ")]
        public decimal Fund { get; set; }
        public int Status { get; set; }
    }
}
