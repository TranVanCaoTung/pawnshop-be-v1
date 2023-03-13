using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class CustomerRelativeRelationship
    {
        public Guid CustomerRelativeRelationshipId { get; set; }
        public Guid CustomerId { get; set; }
        public string RelativeName { get; set; }
        public string RelativeRelationship { get; set; }
        public decimal? Salary { get; set; }
        public string Address { get; set; }
        public string RelativePhone { get; set; }

        
        
        public virtual Customer Customer { get; set; }

    }
}
