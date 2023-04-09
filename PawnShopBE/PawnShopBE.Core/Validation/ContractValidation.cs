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
    public class ContractValidation : AbstractValidator<ContractDTO>
    {
        public ContractValidation()
        {
            RuleFor(b => b.ContractAssetName).NotEmpty().MaximumLength(50).WithMessage("Tên tài sản ko vượt quá 50 ký tự");
            RuleFor(b => b.InsuranceFee).GreaterThan(1000000000).WithMessage("Không nhập số tiền lớn hơn 1 tỷ");
            RuleFor(b => b.Loan).GreaterThan(1000000000).WithMessage("Không nhập số tiền lớn hơn 1 tỷ");
            RuleFor(b => b.TotalProfit).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 triệu");
            RuleForEach(b => b.PawnableAttributeDTOs).ChildRules(a =>
            {
                a.RuleFor(x => x.Description).NotNull().MaximumLength(50).WithMessage("Miêu tả ko vượt quá 50 ký tự");
            });
        }

    }
}
