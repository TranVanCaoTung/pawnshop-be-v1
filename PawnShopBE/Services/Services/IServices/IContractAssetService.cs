using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IContractAssetService
    {
        Task<bool> CreateContractAsset(ContractAsset contractAsset);

        Task<IEnumerable<ContractAsset>> GetAllContractAssets();
        Task<IEnumerable<ContractAsset>> GetContractAssetsByWarehouseId(int warehouseId);

        Task<ContractAsset> GetContractAssetById(int contractAssetId);

        Task<bool> UpdateContractAsset(ContractAsset contractAsset);

        Task<bool> DeleteContractAsset(int contractAssetId);
    }
}
