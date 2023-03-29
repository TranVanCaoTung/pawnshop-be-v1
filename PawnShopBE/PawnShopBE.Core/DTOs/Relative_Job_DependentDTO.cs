using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class Relative_Job_DependentDTO
    {
        public int? KycId { get; set; }

        public string FullName { get; set; }

        public string CCCD { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public int Point { get; set; }
        public ICollection<DependentPeopleDTO>? DependentPeople { get; set; }
        public ICollection<JobDTO>? Jobs { get; set; }
        public ICollection<CustomerRelativeDTO>? CustomerRelativeRelationships { get; set; }
    }
}
