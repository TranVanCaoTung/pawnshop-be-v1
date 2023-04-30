using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/logAsset")]
    [ApiController]
    [Authorize]
    public class LogAssetController : ControllerBase
    {
        private readonly ILogAssetService _logAssetService;
        private readonly IContractService _contractService;
        public LogAssetController(ILogAssetService logAssetService, IContractService contractService)
        {
            _logAssetService = logAssetService;
            _contractService = contractService;
        }

        [HttpPut("updateLogAsset/{logAssetId}")]
        public async Task<IActionResult> UpdateLogAsset(int logAssetId, LogAsset logAsset)
        {
            if (logAsset != null)
            {
                var response = await _logAssetService.UpdateLogAsset(logAssetId, logAsset);
                if (response)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }

        [HttpGet("getLogAssetsByContractId/{contractId}")]
        public async Task<IActionResult> GetLogAssetByContracttId(int contractId)
        {
            var contract = await _contractService.GetContractById(contractId);
            if (contract == null)
            {
                return NotFound();
            }
            var assetId = contract.ContractAssetId;
            var respone = await _logAssetService.LogAssetByAssetId(assetId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return NotFound(respone);
        }
    }
}
