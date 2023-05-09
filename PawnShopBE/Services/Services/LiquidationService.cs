using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
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
        private readonly IContractService _contractService;
        private readonly ILogContractService _logContractService;
        private readonly DbContextClass _dbContextClass;


        public LiquidationService(IUnitOfWork unitOfWork, IContractService contractService, ILogContractService logContractService, DbContextClass dbContextClass)
        {
            _unit = unitOfWork;
            _contractService = contractService;
            _logContractService = logContractService;
            _dbContextClass = dbContextClass;
        }
        public async Task<bool> CreateLiquidation(int contractId, decimal liquidationMoney)
        {
            var liquidationDetail = await GetLiquidationById(contractId);
            var oldContract = await _contractService.GetContractById(contractId);

            if (liquidationDetail != null)
            {
                var liquidation = new Liquidtation();
                liquidation.LiquidationMoney = liquidationMoney;
                liquidation.liquidationDate = liquidationDetail.LiquidationDate;
                liquidation.ContractId = contractId;
                liquidation.Description = "Thanh lý";
                oldContract.ActualEndDate = DateTime.Now;
                oldContract.Status = (int)ContractConst.CLOSE;

                // Close Log Contract
                var contractJoinUserJoinCustomer = from getcontract in _dbContextClass.Contract
                                                   join customer in _dbContextClass.Customer
                                                   on oldContract.CustomerId equals customer.CustomerId
                                                   join user in _dbContextClass.User
                                                   on oldContract.UserId equals user.UserId
                                                   select new
                                                   {
                                                       ContractId = oldContract.ContractId,
                                                       UserName = user.FullName,
                                                       CustomerName = customer.FullName,
                                                   };
                var oldLogContract = new LogContract();
                foreach (var row in contractJoinUserJoinCustomer)
                {
                    oldLogContract.ContractId = row.ContractId;
                    oldLogContract.UserName = row.UserName;
                    oldLogContract.CustomerName = row.CustomerName;
                }
                oldLogContract.Debt = oldContract.Loan;
                oldLogContract.Paid = oldContract.Loan;
                oldLogContract.LogTime = DateTime.Now;
                oldLogContract.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                oldLogContract.EventType = (int)LogContractConst.CLOSE_CONTRACT;

                await _logContractService.CreateLogContract(oldLogContract);
                await _unit.Liquidations.Add(liquidation);
                _unit.Contracts.Update(oldContract);
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

        public async Task<DisplayLiquidationDetail> GetLiquidationById(int contractId)
        {
            var displayLiquidationDetail = new DisplayLiquidationDetail();
            try
            {
                var ContractJoinAsset = from contract in _dbContextClass.Contract
                                        join asset in _dbContextClass.ContractAsset
                                        on contract.ContractAssetId equals asset.ContractAssetId
                                        join pawnableProduct in _dbContextClass.PawnableProduct
                                        on asset.PawnableProductId equals pawnableProduct.PawnableProductId
                                        where contract.ContractId == contractId
                                        select new
                                        {
                                            AssetName = asset.ContractAssetName,
                                            TypeOfProduct = pawnableProduct.TypeOfProduct
                                        };
                foreach (var row in ContractJoinAsset)
                {
                    displayLiquidationDetail.AssetName = row.AssetName;
                    displayLiquidationDetail.TypeOfProduct = row.TypeOfProduct;
                    displayLiquidationDetail.LiquidationDate = DateTime.Now;
                    displayLiquidationDetail.LiquidationMoney = 0;
                }
            }
            catch (Exception e)
            {
                displayLiquidationDetail = null;
            }
            return displayLiquidationDetail;
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
