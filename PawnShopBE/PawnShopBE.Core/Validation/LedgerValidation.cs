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
    public class LedgerValidation : AbstractValidator<LedgerDTO>
    {
        //public LedgerValidation()
        //{
        //    RuleFor(b => b.ReceivedPrincipal).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
        //    RuleFor(b => b.RecveivedInterest).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
        //    RuleFor(b => b.Loan).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
        //    RuleFor(b => b.Balance).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
        //}

    }
}
