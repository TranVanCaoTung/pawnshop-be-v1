using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IDependentService
    {
        Task<bool> CreateDependent(DependentPeople dependent);
        Task<IEnumerable<DependentPeople>> GetDependent();
        Task<DependentPeople> GetDependentById(Guid dependentId);
        Task<bool> UpdateDependent(DependentPeople dependent);
        Task<bool> DeleteDependent(Guid dependentId);
    }
}
