using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
   public class JobDTO
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Tên công việc không được để trống")]
        [StringLength(50, MinimumLength = 6,
      ErrorMessage = "Tên công việc phải dài từ 6 - 50 ký tự")]
        public string NameJob { get; set; }

        [Required(ErrorMessage = "Địa chỉ nơi làm việc không được để trống")]
        public string WorkLocation { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal Salary { get; set; }
        public bool IsWork { get; set; }
        public string LaborContract { get; set; }
    }
}
