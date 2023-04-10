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
    public class LiquidationValidation : AbstractValidator<LiquidationDTO>
    {
        //public LiquidationValidation()
        //{
        //    RuleFor(b => b.Description).MaximumLength(50).WithMessage("Mô tả không được vượt quá 50 ký tự");
        //    RuleFor(b => b.LiquidationMoney).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 Tr");
        //}

    }
}
