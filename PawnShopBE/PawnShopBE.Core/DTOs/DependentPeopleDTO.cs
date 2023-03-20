using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class DependentPeopleDTO
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Tên người phụ thuộc không được để trống")]
        [StringLength(50, MinimumLength = 6,
         ErrorMessage = "Tên người phụ thuộc phải dài từ 6 - 50 ký tự")]
        public string DependentPeopleName { get; set; }

        public string CustomerRelationShip { get; set; }

        [Range(1000, 100000000000, ErrorMessage = "Tiền nhập phải từ 1000 - 1 Tỷ")]
        public decimal? MoneyProvided { get; set; }

        [Required(ErrorMessage = "Địa chỉ người quan hệ không được để trống")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        [StringLength(11, MinimumLength = 10,
            ErrorMessage = "Số điện thoại phải từ 10-11 số")]
        public string PhoneNumber { get; set; }
    }
}
