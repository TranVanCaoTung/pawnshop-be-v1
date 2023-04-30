using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface ILogAssetRepository : IGenericRepository<LogAsset>
    {
        public Task<IEnumerable<LogAsset>> GetAllByAssetId(int contractAssetId);
    }
}
