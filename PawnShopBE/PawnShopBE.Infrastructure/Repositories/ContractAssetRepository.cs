﻿using Microsoft.EntityFrameworkCore;
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
    public class ContractAssetRepository : GenericRepository<ContractAsset>, IContractAssetRepository
    {
        public ContractAssetRepository(DbContextClass dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<ContractAsset>> GetContractAssetByWarehouseId(int warehouseId)
        {
            return await _dbContext.Set<ContractAsset>()
            .Where(a => a.WarehouseId == warehouseId).ToListAsync();
        }
    }
}
