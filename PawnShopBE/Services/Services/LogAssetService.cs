using AutoMapper;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LogAssetService : ILogAssetService

    {
        private readonly IUnitOfWork _unit;
        private readonly ILogAssetRepository _logAssetRepository;
        private readonly IMapper _mapper;
        private readonly IContractAssetService _contractAssetService;
        public LogAssetService(IUnitOfWork unit, ILogAssetRepository logAssetRepository, IMapper mapper, IContractAssetService contractAssetService)
        {
            _unit = unit;
            _logAssetRepository = logAssetRepository;
            _mapper = mapper;
            _contractAssetService = contractAssetService;
        }
        public async Task<bool> CreateLogAsset(LogAsset logAsset)
        {
            await _unit.LogAssets.Add(logAsset);
            var result = _unit.Save();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<DisplayLogAsset>> LogAssetByAssetId(int contractAssetId)
        {
            var logAssetList = await _logAssetRepository.GetAllByAssetId(contractAssetId);
            var contractAsset = await _contractAssetService.GetContractAssetById(contractAssetId);

            var displayLogAssetList = new List<DisplayLogAsset>();

            foreach ( var logAsset in logAssetList)
            {
                var displayLogAsset = new DisplayLogAsset();
                displayLogAsset.logAssetId = logAsset.logAssetId;
                displayLogAsset.AssetName = contractAsset.ContractAssetName;
                displayLogAsset.WareHouseName = logAsset.WareHouseName;
                displayLogAsset.UserName = logAsset.UserName;
                displayLogAsset.contractAssetId = contractAssetId;
                displayLogAsset.Description = logAsset.Description;
                displayLogAsset.ImportImg = logAsset.ImportImg;
                displayLogAsset.ExportImg = logAsset.ExportImg;
                displayLogAsset.CreateDate = logAsset.CreateDate;
                displayLogAssetList.Add(displayLogAsset);
            }
            return (displayLogAssetList != null) ? displayLogAssetList.OrderByDescending(x => x.CreateDate) : null;
        }

        public async Task<bool> UpdateLogAsset(int logAssetId, LogAsset logAsset)
        {
            var logAssetUpdate =  await _unit.LogAssets.GetById(logAssetId);
            if (logAssetUpdate != null)
            {
                logAssetUpdate.ImportImg = logAsset.ImportImg;
                logAssetUpdate.ExportImg = logAsset.ExportImg;
                logAssetUpdate.Description = logAsset.Description;
                _unit.LogAssets.Update(logAssetUpdate);
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
