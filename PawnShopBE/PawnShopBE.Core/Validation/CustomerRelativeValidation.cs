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
    public class CustomerRelativeValidation : AbstractValidator<CustomerRelativeDTO>
    {
        //public CustomerRelativeValidation()
        //{
        //    RuleFor(b => b.RelativeName).NotEmpty().MaximumLength(50).WithMessage("Tên khách hàng không được vượt quá 50 ký tự");
        //    RuleFor(b => b.Address).NotNull().MaximumLength(50).WithMessage("Địa chỉ không được vượt quá 50 ký tự");
        //    RuleFor(b => b.RelativePhone).Matches(@"^\+?\d{10,12}$").WithMessage("Nhập đúng định dạng số điện thoại, 10 số");
        //    RuleFor(b => b.Salary).GreaterThan(1000000000).WithMessage("Không nhập số tiền lớn hơn 1 tỷ");
        //}

    }
}
