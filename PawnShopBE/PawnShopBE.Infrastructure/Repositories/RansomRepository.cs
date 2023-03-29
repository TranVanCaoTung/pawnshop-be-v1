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
    public class RansomRepository : GenericRepository<Ransom>, IRansomRepository
    {
        public RansomRepository (DbContextClass dbContext) : base(dbContext)
        {

        }

        public async Task<Ransom> GetRanSomByContractId(int contractId)
        {
            Ransom ransom = _dbContext.Ransom.FirstOrDefault(c => c.ContractId == contractId);
            return ransom;
        }
    }
}
