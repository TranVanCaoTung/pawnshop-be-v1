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
    public class ContractAssetValidator : AbstractValidator<ContractAssetDTO>
    {
        //public ContractAssetValidator()
        //{
        //    RuleFor(b => b.ContractAssetName).NotEmpty().MaximumLength(50).WithMessage("Tên tài sản ko vượt quá 50 ký tự");
        //    RuleFor(b => b.Description).NotNull().MaximumLength(50).WithMessage("Miêu tả ko vượt quá 50 ký tự");
        //    RuleFor(b => b.Image).NotEmpty().WithMessage("Cần có hình ảnh tài sản"); ;
        //}

    }
}
