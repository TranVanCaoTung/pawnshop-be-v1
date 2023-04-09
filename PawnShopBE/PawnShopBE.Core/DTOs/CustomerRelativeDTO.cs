using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
   public class CustomerRelativeDTO
    {
        public Guid CustomerId { get; set; }
        public Guid CustomerRelativeRelationshipId { get; set; }

        public string RelativeRelationship { get; set; }

        public string RelativeName { get; set; }

       
        public decimal? Salary { get; set; }

        
        public string Address { get; set; }

        public string RelativePhone { get; set; }
    }
}
