using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class BranchDTO
    {
        public int BranchId { get; set; }

   
        public string BranchName { get; set; }

        public string Address { get; set; }

        
        public string PhoneNumber { get; set; }

        public decimal Fund { get; set; }
        public int Status { get; set; }
    }
}
