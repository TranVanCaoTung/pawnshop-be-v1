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
    public class DiaryImgRepository : GenericRepository<DiaryImg>, IDiaryImgRepository
    {
        private readonly DbContextClass _dbContext;
        public DiaryImgRepository(DbContextClass dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DiaryImg>> GetDiaryImgByInterestDiaryId(int interestDiaryId)
        {
            
            return await _dbContext.Set<DiaryImg>()
            .Where(e => e.InterestDiaryId == interestDiaryId)
            .ToListAsync();

        }
    }
}
