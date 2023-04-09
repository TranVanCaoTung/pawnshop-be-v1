using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface ILogContractService
    {
        Task<bool> CreateLogContract(LogContract logContract);
        Task<IEnumerable<LogContract>> GetLogContracts(int num);
        Task<IEnumerable<LogContract>> LogContractsByContractId(int contractId);
        Task<IEnumerable<LogContract>> LogContractsByBranchId(int branchId);
    }
}
