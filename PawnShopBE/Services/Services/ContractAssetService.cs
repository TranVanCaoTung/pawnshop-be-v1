using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ContractAssetService : IContractAssetService
    {
        public IUnitOfWork _unitOfWork;
        private readonly ContractAsset contractAsset;
        private readonly IContractAssetRepository _contractAssetRepository;

        public ContractAssetService(IUnitOfWork unitOfWork, IContractAssetRepository contractAssetRepository)
        {
            _unitOfWork = unitOfWork;
            _contractAssetRepository = contractAssetRepository;
        }
        public async Task<bool> CreateContractAsset(ContractAsset contractAsset)
        {
            if (contractAsset != null)
            {
                contractAsset.Status = (int)ContractAssetConst.IN_STOCK;
                await _unitOfWork.ContractAssets.Add(contractAsset);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteContractAsset(int contractAssetId)
        {
            var contractAssetDelete = _unitOfWork.ContractAssets.SingleOrDefault
                (contractAsset, j => j.ContractAssetId == contractAssetId);
            if (contractAssetDelete != null)
            {
                _unitOfWork.ContractAssets.Delete(contractAssetDelete);
                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<ContractAsset>> GetAllContractAssets()
        {
            var result = await _unitOfWork.ContractAssets.GetAll();
            return result;
        } 

        public async Task<ContractAsset> GetContractAssetById(int contractAssetId)
        {
            var result = await _unitOfWork.ContractAssets.GetById(contractAssetId);
            return result;
        }

        public async Task<bool> UpdateContractAsset(ContractAsset contractAsset)
        {
            var contractAssetUpdate = _unitOfWork.ContractAssets.SingleOrDefault
                (contractAsset, j => j.ContractAssetId == contractAsset.ContractAssetId);
            if (contractAssetUpdate != null)
            {
                contractAssetUpdate.WarehouseId = contractAsset.WarehouseId;
                contractAssetUpdate.Status = contractAsset.Status;
                _unitOfWork.ContractAssets.Update(contractAssetUpdate);
                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    //var logAsset = new LogAsset();
                    //logAsset.contractAssetId = contractAssetUpdate.ContractAssetId;
                    //logAsset.Description = null;
                    //logAsset.ImportImg = null;
                    //logAsset.ExportImg = null;
                    //logAsset.UserName = GetUser(contract.UserId);
                    //logAsset.WareHouseName =  .WarehouseName;
                    //await _logAssetService.CreateLogAsset(logAsset);

                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<ContractAsset>> GetContractAssetsByWarehouseId(int warehouseId)
        {
            if (warehouseId != null)
            {
                var assetList = await _contractAssetRepository.GetContractAssetByWarehouseId(warehouseId);
                return (List<ContractAsset>) assetList;
            }
            return null; 
        }
    }
}
