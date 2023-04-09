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
    public class DiaryValidation : AbstractValidator<InterestDiaryDTO>
    {
        public DiaryValidation()
        {
            RuleFor(b => b.Description).MaximumLength(50).WithMessage("Tên tài sản ko vượt quá 50 ký tự");
            RuleFor(b => b.Payment).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
            RuleFor(b => b.Penalty).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
            RuleFor(b => b.TotalPay).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
            RuleFor(b => b.PaidMoney).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
        }

    }
}
