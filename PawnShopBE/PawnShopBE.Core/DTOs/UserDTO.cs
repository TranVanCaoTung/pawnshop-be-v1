using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public int? RoleId { get; set; }
        public int? BranchId { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(20, MinimumLength = 6, 
            ErrorMessage = "Mật khẩu phải dài từ 6 - 20 ký tự, có chữ và số")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage ="Nhập đúng định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Tên nhân viên phải dài từ 6 - 50 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ của nhân viên không được để trống")]
        public string Address { get; set; }

        [Phone(ErrorMessage ="Nhập đúng định dạng số điện thoại")]
        [StringLength(11, MinimumLength = 10,
            ErrorMessage = "Số điện thoại phải từ 10-11 số")]
        public string Phone { get; set; }
        public int Status { get; set; }
    }  
}
