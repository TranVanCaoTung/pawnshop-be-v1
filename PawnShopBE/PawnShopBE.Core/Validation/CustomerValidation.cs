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
    public class CustomerValidation : AbstractValidator<CustomerDTO>
    {
        //public CustomerValidation()
        //{
        //    RuleFor(b => b.FullName).NotEmpty().MaximumLength(50).WithMessage("Tên khách hàng không được vượt quá 50 ký tự");
        //    RuleFor(b => b.CCCD).NotNull().Matches(@"^\d{10,14}$").WithMessage("Nhập đúng định dạng CCCD, 10-14 số"); ;
        //    RuleFor(b => b.Address).NotNull().MaximumLength(50).WithMessage("Địa chỉ không được vượt quá 50 ký tự");
        //    RuleFor(b => b.Phone).Matches(@"^\+?\d{10,12}$").WithMessage("Nhập đúng định dạng số điện thoại, 10 số");
           
        //}

    }
}
