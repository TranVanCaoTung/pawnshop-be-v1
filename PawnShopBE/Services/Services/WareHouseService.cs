using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Services.Services
{
    public class WareHouseService : IWareHouseService
    {
        private readonly IUnitOfWork _unit;
        private readonly Warehouse warehouse;
        private readonly IContractAssetService _asset;
        private readonly IPawnableProductService _pawnable;
        private IMapper _mapper;
        
        public WareHouseService(IUnitOfWork unitOfWork, IContractAssetService asset,
            IPawnableProductService pawnable,IMapper mapper) { 
              _unit=unitOfWork;
            _asset=asset;
            _pawnable=pawnable;
            _mapper=mapper;
        }
        
        public async Task<WareHouseDTO> getWareHouseDetail(int id,int num)
        {
            //get List Asset
            var listAsset= await _asset.GetAllContractAssets();
            var asset = from l in listAsset where l.WarehouseId== id select l;

            //get pawnable
            var getPawnableId = from l in asset select l.PawnableProductId;
            var pawnableID= getPawnableId.First();
            var listPawnable= await _pawnable.GetAllPawnableProducts(0);
            var pawnable= from p in listPawnable where p.PawnableProductId==pawnableID select p;

            //get wareHouse Detail
            var wareHouseDTO = new WareHouseDTO();
            wareHouseDTO.ContractAssets=new List<ContractAssetDTO>();
            wareHouseDTO=getAssetDTO(wareHouseDTO,asset,pawnable);

            //Mapper Asset To Skip Take PAge
            var assetMaper = _mapper.Map<IEnumerable<ContractAsset>>(wareHouseDTO.ContractAssets);
            var takeAsset =await _unit.ContractAssets.TakePage(num, assetMaper);
            var assetDTO = _mapper.Map<ICollection<ContractAssetDTO>>(takeAsset);
            wareHouseDTO.ContractAssets = assetDTO;

            return wareHouseDTO;
        }

        private WareHouseDTO getAssetDTO(WareHouseDTO wareHouseDTO, IEnumerable<ContractAsset> asset,
            IEnumerable<PawnableProduct> pawnable)
        {
            foreach (var x in asset)
            {
                var assetDTO = new ContractAssetDTO();
                assetDTO.ContractAssetName = x.ContractAssetName;
                assetDTO.Description = x.Description;
                assetDTO.Image = x.Image;
                foreach (var y in pawnable)
                {
                    assetDTO.commodifyCode = y.CommodityCode;
                }
                wareHouseDTO.ContractAssets.Add(assetDTO);
            }
            return wareHouseDTO;
        }

        public async Task<bool> CreateWareHouse(Warehouse warehouse)
        {
            if ( warehouse!= null)
            {
                await _unit.Warehouses.Add(warehouse);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteWareHouse(int wareHouseId)
        {

           var wareHouseDelete=  _unit.Warehouses.
                SingleOrDefault(warehouse,j=> j.WarehouseId == wareHouseId);
            if (wareHouseDelete != null)
            {
                _unit.Warehouses.Delete(wareHouseDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Warehouse>> GetWareHouse(int num)
        {
            var listWarehouse = await _unit.Warehouses.GetAll();
            if (num == 0)
            {
                return listWarehouse;
            }
            var result = await _unit.Warehouses.TakePage(num,listWarehouse);
            return result;
        }

        public async Task<Warehouse> GetWareHouseById(int warehouseId)
        {
            if (warehouseId != null)
            {
                var warehouse = await _unit.Warehouses.GetById(warehouseId);
                if (warehouse != null)
                {
                    return warehouse;
                }
            }
            return null;
        }

        public async Task<bool> UpdateWareHouse(Warehouse warehouse)
        {
            var wareHouseUpdate = _unit.Warehouses.SingleOrDefault
                (warehouse, j => j.WarehouseId == warehouse.WarehouseId);
            if(wareHouseUpdate!= null)
            {
                wareHouseUpdate.WarehouseName=warehouse.WarehouseName;
                wareHouseUpdate.WarehouseAddress=warehouse.WarehouseAddress;
                wareHouseUpdate.Status=warehouse.Status;
                _unit.Warehouses.Update(wareHouseUpdate);
                var result= _unit.Save();
                if(result> 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
