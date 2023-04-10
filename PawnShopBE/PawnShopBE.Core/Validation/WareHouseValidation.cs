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
    public class WareHouseValidation : AbstractValidator<WareHouseDTO>
    {
        //public WareHouseValidation()
        //{
        //    RuleFor(b => b.WarehouseName).NotEmpty().MaximumLength(30).WithMessage("Tên nhà kho không được vượt quá 30 ký tự");
        //    RuleFor(b => b.WarehouseAddress).NotNull().MaximumLength(50).WithMessage("Địa chỉ nhà kho không được vượt quá 50 ký tự");
        //    RuleForEach(b=> b.ContractAssets).SetValidator(new ContractAssetValidator());
        //}

    }
}
