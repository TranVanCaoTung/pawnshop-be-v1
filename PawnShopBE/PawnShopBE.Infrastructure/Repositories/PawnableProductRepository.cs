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
    public class PawnableProductRepository : GenericRepository<PawnableProduct>, IPawnableProductRepository
    {
        public PawnableProductRepository(DbContextClass dbContext) : base(dbContext)
        {

        }

    }
}
