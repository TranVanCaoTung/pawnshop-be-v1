using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface ILogContractRepository : IGenericRepository<LogContract>
    {
        Task<IEnumerable<LogContract>> getLogContractsByContractId(int contractId);
        Task<IEnumerable<LogContract>> getLogContractsByBranchId(int branchId);

    }
}
