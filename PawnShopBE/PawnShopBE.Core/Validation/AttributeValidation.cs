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
    public class AttributeValidation : AbstractValidator<AttributeDTO>
    {
        //public AttributeValidation()
        //{
        //    RuleFor(b => b.Description).NotNull().MaximumLength(50).WithMessage("Miêu tả ko vượt quá 50 ký tự");
        //}

    }
}
