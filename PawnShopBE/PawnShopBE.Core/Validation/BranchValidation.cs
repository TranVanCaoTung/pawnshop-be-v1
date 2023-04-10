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
    public class BranchValidation:AbstractValidator<BranchDTO>
    {
        //public BranchValidation()
        //{
        //    RuleFor(b => b.BranchName).NotEmpty().MaximumLength(20).WithMessage("Tên cửa hàng ko vượt quá 20 ký tự");
        //    RuleFor(b => b.Address).NotEmpty().MaximumLength(50).WithMessage("Địa chỉ ko vượt quá 50 ký tự");
        //    RuleFor(b => b.PhoneNumber).Matches(@"^\+?\d{10,12}$").WithMessage("Nhập đúng định dạng số điện thoại, 10 số"); 
        //    RuleFor(b => b.Fund).GreaterThan(10000).WithMessage("Không nhập số tiền lớn hơn 10 tỷ");
        //}

    }
}
