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
    public class MoneyValidation : AbstractValidator<MoneyDTO>
    {
        public MoneyValidation()
        {
            RuleFor(b => b.MoneyInput).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 Tr");
        }

    }
}
