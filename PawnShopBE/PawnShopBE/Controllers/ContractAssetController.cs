using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/contractAsset")]
    [ApiController]
    [Authorize]
    public class ContractAssetController : ControllerBase
    {
        private readonly IContractAssetService _contractAssetService;
        private readonly IMapper _mapper;

        public ContractAssetController(IContractAssetService contractAssetService, IMapper mapper) 
        { 
        _contractAssetService=contractAssetService;
            _mapper=mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllContractAsset() {
            var respone =await _contractAssetService.GetAllContractAssets();
            return (respone != null) ? Ok(respone) : BadRequest();
        }
        
        [HttpPost("createContractAsset")]
        public async Task<IActionResult> CreateContractAsset( ContractAssetDTO contractAsset)
        {
            var contractAssetMapper = _mapper.Map<ContractAsset>(contractAsset);
            var respone = await _contractAssetService.CreateContractAsset(contractAssetMapper);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }
        [HttpDelete("deleteContractAsset/{id}")]
        public async Task<IActionResult> DeleteContractAsset(int id)
        {
            var respone = await _contractAssetService.DeleteContractAsset(id);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }
        [HttpPut("updateContractAsset/{userId}")]
        public async Task<IActionResult> UpdateContractAsset(ContractAssetDTO contractAsset, Guid userId)
        {
            var contractAssetUpdate=_mapper.Map<ContractAsset>(contractAsset);
            var respone = await _contractAssetService.UpdateContractAsset(userId, contractAssetUpdate);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }

        [HttpGet("assets/{warehouseId}")]
        public async Task<IActionResult> GetAssetByWarehouseId(int warehouseId)
        {
            var respone = await _contractAssetService.GetContractAssetsByWarehouseId(warehouseId);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }
    }
}
