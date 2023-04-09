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
        public int? InterestDiaryId { get; set; }

        public decimal Payment { get; set; }
        public decimal? Penalty { get; set; }
        public decimal TotalPay { get; set; }
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
