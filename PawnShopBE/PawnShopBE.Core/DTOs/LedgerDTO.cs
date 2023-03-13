using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class LedgerDTO
    {
        public int LedgerId { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal ReceivedPrincipal { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal RecveivedInterest { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal Loan { get; set; }
        public long Balance { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
    }
}
