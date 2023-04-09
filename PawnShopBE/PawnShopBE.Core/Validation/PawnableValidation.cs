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
    public class PawnableValidation : AbstractValidator<PawnableDTO>
    {
        public PawnableValidation()
        {
            RuleFor(b => b.TypeOfProduct).NotEmpty().MaximumLength(30).WithMessage("Thể loại không được vượt quá 30 ký tự");
            RuleFor(b => b.CommodityCode).NotEmpty().MaximumLength(10).WithMessage("Mã Code không được vượt quá 10 ký tự");
            RuleForEach(b => b.AttributeDTOs).SetValidator(new AttributeValidation());
        }

    }
}
