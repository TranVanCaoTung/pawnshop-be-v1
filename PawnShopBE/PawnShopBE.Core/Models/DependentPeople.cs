using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class DependentPeople
    {
        public Guid DependentPeopleId { get; set; }
        public Guid CustomerId { get; set; }
        public string DependentPeopleName { get; set; }
        public string CustomerRelationShip { get; set; }
        public decimal? MoneyProvided { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }



        public virtual Customer Customer { get; set; }

    }
}
