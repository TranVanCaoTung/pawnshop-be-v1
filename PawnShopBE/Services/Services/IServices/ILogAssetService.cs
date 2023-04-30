using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface ILogAssetService
    {
        Task<bool> CreateLogAsset(LogAsset logAsset);
        Task<IEnumerable<DisplayLogAsset>> LogAssetByAssetId(int contractAssetId);
        Task<bool> UpdateLogAsset(int logAssetId, LogAsset LogAsset);

    }
}
