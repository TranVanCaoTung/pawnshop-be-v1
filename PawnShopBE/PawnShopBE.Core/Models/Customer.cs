using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public int KycId { get; set; }
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public int Point { get; set; }



        public virtual Kyc? Kyc { get; set; }
        public ICollection<Contract>? Contracts { get; set; }
        public ICollection<DependentPeople>? DependentPeople { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<CustomerRelativeRelationship>? CustomerRelativeRelationships { get; set; }
        public Customer()
        {
            Contracts = new List<Contract>();
            DependentPeople = new List<DependentPeople>();
            Jobs = new List<Job>();
            CustomerRelativeRelationships = new List<CustomerRelativeRelationship>();
        }
    }
}
