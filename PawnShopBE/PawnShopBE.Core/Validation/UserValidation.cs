using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PawnShopBE.Core.DTOs;

namespace PawnShopBE.Core.Validation
{
    public class UserValidation: AbstractValidator<UserDTO>
    {
        public UserValidation()
        {
            RuleFor(b => b.FullName).NotEmpty().MaximumLength(50).WithMessage("Tên nhân viên không được vượt quá 50 ký tự");
            RuleFor(b => b.UserName).NotNull().MaximumLength(30).WithMessage("Tài khoản không được vượt quá 30 ký tự");
            RuleFor(b => b.Password).NotNull().Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{10,}$")
                .WithMessage("Mật khẩu cần ít nhất 10 ký tự, bao gồm chữ và số");
            RuleFor(b => b.Address).NotNull().MaximumLength(50).WithMessage("Địa chỉ không được vượt quá 50 ký tự");
            RuleFor(b => b.Phone).Matches(@"^\+?\d{10,12}$").WithMessage("Nhập đúng định dạng số điện thoại, 10 số");
            RuleFor(b=> b.Email).NotNull().Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").
                WithMessage("Nhập đúng định dạng email, ex: example@email.com");

        }

    }
}
