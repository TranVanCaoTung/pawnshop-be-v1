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
    public class DependentValidation : AbstractValidator<DependentPeopleDTO>
    {
        //public DependentValidation()
        //{
        //    RuleFor(b => b.DependentPeopleName).NotEmpty().MaximumLength(50).WithMessage("Tên khách hàng không được vượt quá 50 ký tự");
        //    RuleFor(b => b.Address).NotNull().MaximumLength(50).WithMessage("Địa chỉ không được vượt quá 50 ký tự");
        //    RuleFor(b => b.PhoneNumber).Matches(@"^\+?\d{10,12}$").WithMessage("Nhập đúng định dạng số điện thoại, 10 số");
        //    RuleFor(b => b.MoneyProvided).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 Tr");
        //}

    }
}
