using PawnShopBE.Core.Display;
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
        Task<bool> CreateLiquidation(int contractId,Guid userId, decimal liquidationMoney, string proofImg);
        Task<IEnumerable<Liquidtation>> GetLiquidation();
        Task<DisplayLiquidationDetail> GetLiquidationById(int contractId);
        Task<bool> UpdateLiquidation(Liquidtation liquidtation);
        Task<bool> DeleteLiquidation(int liquidationId);
    }
}
