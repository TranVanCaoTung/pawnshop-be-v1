using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class LogContractRepository : GenericRepository<LogContract>, ILogContractRepository
    {
        public LogContractRepository(DbContextClass dbContext) : base(dbContext)
        {

        }

        public async Task<LogContract> getLogContractByContractId(int contractId)
        {
            var logContract = _dbContext.LogContracts.FirstOrDefault(x => x.ContractId == contractId);
            return logContract;
        }
    }
}
