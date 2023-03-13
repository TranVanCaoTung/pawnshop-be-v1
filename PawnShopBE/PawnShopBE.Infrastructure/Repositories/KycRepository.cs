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
    public class KycRepository : GenericRepository<Kyc>, IKycRepository
    {
        public KycRepository(DbContextClass dbContext) : base(dbContext)
        {

        }

    }
}
