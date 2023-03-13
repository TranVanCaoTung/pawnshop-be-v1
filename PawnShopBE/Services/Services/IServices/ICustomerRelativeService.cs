using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface ICustomerRelativeService
    {
        Task<bool> CreateCustomerRelative(CustomerRelativeRelationship customerRelative);
        Task<IEnumerable<CustomerRelativeRelationship>> GetCustomerRelative();
        Task<Job> GetCustomerRelativeById(Guid CustomerRelativeId);
        Task<bool> UpdateCustomerRelative(CustomerRelativeRelationship customerRelative);
        Task<bool> DeleteCustomerRelative(Guid CustomerRelativeId);
    }
}
