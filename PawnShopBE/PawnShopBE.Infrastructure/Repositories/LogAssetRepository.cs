using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class LogAssetRepository : GenericRepository<LogAsset>, ILogAssetRepository
    {
        private readonly DbContextClass _dbContext;

        public LogAssetRepository (DbContextClass dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LogAsset>> GetAllByAssetId(int contractAssetId)
        {
            return await _dbContext.Set<LogAsset>()
            .Where(e => e.contractAssetId == contractAssetId)
            .ToListAsync();
        }
    }
}
