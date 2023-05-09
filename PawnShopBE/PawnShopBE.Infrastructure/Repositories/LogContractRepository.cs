using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<LogContract>> getLogContractsByContractId(int contractId)
        {
            return await _dbContext.Set<LogContract>()
            .Where(e => e.ContractId == contractId)
            .ToListAsync();
        }
        public async Task<IEnumerable<LogContract>> getLogContractsByBranchId(int branchId)
        {
            var contractByBranchId = await _dbContext.Set<Contract>()
                .Where(c => c.BranchId == branchId).ToListAsync();
            var logContractList = new List<LogContract>();
            foreach (var contract in contractByBranchId)
            {
                var logContractByBranchId = await _dbContext.Set<LogContract>()
                    .Where(e => e.ContractId == contract.ContractId && e.LogTime.Month == DateTime.Today.Month)
                    .ToListAsync();
                foreach (var logContract in logContractByBranchId)
                {
                    logContractList.Add(logContract);
                }
            }
            return logContractList;
        }
    }
}
