using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class InterestDiaryRepository : GenericRepository<InterestDiary>, IInterestDiaryRepository
    {
        private readonly DbContextClass _dbContext;

        public InterestDiaryRepository(DbContextClass dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<InterestDiary>> GetDiaryByContractId(int contractId)
        {
            return await _dbContext.Set<InterestDiary>()
            .Where(e => e.ContractId == contractId)
            .ToListAsync();
        }
    }
}
