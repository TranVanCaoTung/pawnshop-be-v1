using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LiquidationService : ILiquidationService
    {
        private readonly IUnitOfWork _unit;
        private readonly Liquidtation liquidtationDb;

        public LiquidationService(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        public async Task<bool> CreateLiquidation(Liquidtation liquidtation)
        {
            if (liquidtation != null)
            {
                await _unit.Liquidations.Add(liquidtation);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteLiquidation(int liquidationId)
        {
            var liquidationDelete = _unit.Liquidations.SingleOrDefault
                (liquidtationDb, j => j.LiquidationId == liquidationId);
            if (liquidationDelete != null)
            {
                _unit.Liquidations.Delete(liquidationDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Liquidtation>> GetLiquidation()
        {
            var result = await _unit.Liquidations.GetAll();
            return result;
        }

        public Task<Liquidtation> GetLiquidationById(int liquidationId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateLiquidation(Liquidtation liquidtation)
        {
            var liquidtationUpdate = _unit.Liquidations.SingleOrDefault
                (liquidtation, j => j.LiquidationId == liquidtation.LiquidationId);
            if (liquidtationUpdate != null)
            {
                liquidtationUpdate.ContractId = liquidtation.ContractId;
                liquidtationUpdate.LiquidationMoney = liquidtation.LiquidationMoney;
                liquidtationUpdate.liquidationDate = liquidtation.liquidationDate;
                liquidtationUpdate.Description = liquidtation.Description;
                _unit.Liquidations.Update(liquidtationUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
