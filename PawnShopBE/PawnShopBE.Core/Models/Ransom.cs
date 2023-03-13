using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Ransom
    {
        public int RansomId { get; set; }
        public decimal Payment { get; set; }
        public decimal Penalty { get; set; }
        public decimal TotalPay { get; set; }
        public decimal PaidMoney { get; set; }
        public DateTime? PaidDate { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public string? ProofImg { get; set; }

        public virtual Contract? Contract { get; set; }
        public int ContractId { get; set; }

    }
}
