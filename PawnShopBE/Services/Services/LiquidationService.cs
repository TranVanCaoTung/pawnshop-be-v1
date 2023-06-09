﻿using PawnShopBE.Core.Const;
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
        public async Task<bool> CreateLiquidation(int contractId,Guid userId, decimal liquidationMoney, string proofImg)
        {
            var contract = await _contractService.GetContractById(contractId);
            if (contract != null)
            {
                var liquidation = new Liquidtation();
                liquidation.LiquidationMoney = liquidationMoney;
                liquidation.liquidationDate = DateTime.Now;
                liquidation.ContractId = contractId;
                liquidation.Description = proofImg;

                // Change status contract to close
                contract.ActualEndDate = DateTime.Now;
                contract.Status = (int)ContractConst.CLOSE;

                // Close Log Contract
                var contractJoinUserJoinCustomer = from getcontract in _dbContextClass.Contract
                                                   join customer in _dbContextClass.Customer
                                                   on contract.CustomerId equals customer.CustomerId
                                                   select new
                                                   {
                                                       ContractId = contract.ContractId,
                                                       CustomerName = customer.FullName,
                                                   };
                var user = await _unit.Users.GetById(userId);
                var oldLogContract = new LogContract();
                foreach (var row in contractJoinUserJoinCustomer)
                {
                    oldLogContract.ContractId = row.ContractId;
                    oldLogContract.UserName = user.UserName;
                    oldLogContract.CustomerName = row.CustomerName;
                }
                oldLogContract.Debt = contract.Loan;
                oldLogContract.Paid = liquidationMoney;
                oldLogContract.LogTime = DateTime.Now;
                oldLogContract.Description = "Hợp đồng thanh lý với số tiền " + (int)liquidationMoney + " VND.";
                oldLogContract.EventType = (int)LogContractConst.CLOSE_CONTRACT;

                await _logContractService.CreateLogContract(oldLogContract);
                await _unit.Liquidations.Add(liquidation);
                _unit.Contracts.Update(contract);
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
                                        join liquidation in _dbContextClass.Liquidtation
                                        on contract.ContractId equals liquidation.ContractId
                                        where contract.ContractId == contractId
                                        select new
                                        {
                                            AssetName = asset.ContractAssetName,
                                            TypeOfProduct = pawnableProduct.TypeOfProduct,
                                            LiquidationMoney = liquidation.LiquidationMoney,
                                            CreatedDate = liquidation.liquidationDate,
                                            Description = liquidation.Description
                                        };
                foreach (var row in ContractJoinAsset)
                {
                    displayLiquidationDetail.AssetName = row.AssetName;
                    displayLiquidationDetail.TypeOfProduct = row.TypeOfProduct;
                    displayLiquidationDetail.LiquidationDate = row.CreatedDate;
                    displayLiquidationDetail.LiquidationMoney = row.LiquidationMoney;
                    displayLiquidationDetail.Description = row.Description;
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
                liquidtationUpdate.liquidationDate = liquidtation.liquidationDate.Date;
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
