using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface ILiquidationService
    {
        Task<bool> CreateLiquidation(Liquidtation liquidtation);
        Task<IEnumerable<Liquidtation>> GetLiquidation();
        Task<Liquidtation> GetLiquidationById(int liquidationId);
        Task<bool> UpdateLiquidation(Liquidtation liquidtation);
        Task<bool> DeleteLiquidation(int liquidationId);
    }
}
