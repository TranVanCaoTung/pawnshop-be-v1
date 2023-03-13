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
    public class AttributeRepository : GenericRepository<Core.Models.Attribute>, IAttributeRepository
    {
        public AttributeRepository(DbContextClass dbContext) : base(dbContext)
        {

        }

        public async Task<List<Core.Models.Attribute>> GetAttributesByPawnableId(int pawmableProductId)
        {
            return await _dbContext.Attribute
                    .Where(p => p.PawnableProductId == pawmableProductId)
                    .ToListAsync();
        }
    }
}
