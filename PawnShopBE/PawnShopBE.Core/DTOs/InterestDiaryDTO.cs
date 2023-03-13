using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
   public class InterestDiaryDTO
    {
        public int ContractId { get; set; }

        [Range(1000, 10000000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal Payment { get; set; }
        [Range(1000, 10000000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal? Penalty { get; set; }
        [Range(1000, 10000000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal TotalPay { get; set; }
        [Range(1000, 10000000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal? PaidMoney { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime NextDueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public int? PaymentMethod { get; set; }
        public string? ProofImg { get; set; }
    }
}
