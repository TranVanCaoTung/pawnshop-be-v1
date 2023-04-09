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
    public class JobValidation : AbstractValidator<JobDTO>
    {
        public JobValidation()
        {
            RuleFor(b => b.NameJob).NotEmpty().MaximumLength(50).WithMessage("Tên công việc không được vượt quá 50 ký tự");
            RuleFor(b => b.WorkLocation).NotNull().MaximumLength(50).WithMessage("Địa chỉ làm việc không được vượt quá 50 ký tự");
            RuleFor(b => b.Salary).GreaterThan(100000000).WithMessage("Không nhập số tiền lớn hơn 100 Tr");
        }

    }
}
