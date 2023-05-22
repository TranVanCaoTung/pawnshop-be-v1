using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/liquidation")]
    [ApiController]
    [Authorize]
    public class LiquidationController : ControllerBase
    {
        private readonly ILiquidationService _liquidationService;
        private readonly IMapper _mapper;

        public LiquidationController(ILiquidationService liquidationService, IMapper mapper)
        {
            _liquidationService = liquidationService;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllLiquidation()
        {
            var respone = await _liquidationService.GetLiquidation();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("save/{contractId}/{userId}")]
        public async Task<IActionResult> CreateLiquidation(int contractId, Guid userId, decimal liquidationMoney, string proofImg)
        {
           
            var respone = await _liquidationService.CreateLiquidation(contractId, userId, liquidationMoney, proofImg);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteLiquidation/{id}")]
        public async Task<IActionResult> DeleteLiquidation(int id)
        {
            var respone = await _liquidationService.DeleteLiquidation(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateLiquidation")]
        public async Task<IActionResult> UpdateLiquidation(LiquidationDTO liquidation)
        {
            var liquidationUpdate = _mapper.Map<Liquidtation>(liquidation);
            var respone = await _liquidationService.UpdateLiquidation(liquidationUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpGet("detail/{contractId}")]
        public async Task<IActionResult> GetDetail(int contractId)
        {
            var respone = await _liquidationService.GetLiquidationById(contractId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return NotFound();
        }
    }
}
