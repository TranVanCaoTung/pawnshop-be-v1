using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class LiquidationController : ControllerBase
    {
        private readonly ILiquidationService _liquidationService;
        private readonly IMapper _mapper;

        public LiquidationController(ILiquidationService liquidationService, IMapper mapper) 
        { 
        _liquidationService=liquidationService;
            _mapper=mapper;
        }

        [HttpGet("liquidation")]
        public async Task<IActionResult> GetAllLiquidation() {
            var respone =await _liquidationService.GetLiquidation();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        private Validation<LiquidationDTO> _validation;
        
    [HttpPost("liquidation")]
        public async Task<IActionResult> CreateLiquidation(LiquidationDTO liquidation)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(liquidation);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var liquidationMapper = _mapper.Map<Liquidtation>(liquidation);
            var respone = await _liquidationService.CreateLiquidation(liquidationMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("liquidation/{id}")]
        public async Task<IActionResult> DeleteLiquidation(int id)
        {
            var respone = await _liquidationService.DeleteLiquidation(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("liquidation/{id}")]
        public async Task<IActionResult> UpdateLiquidation(int id,LiquidationDTO liquidation)
        {
            var liquidationUpdate=_mapper.Map<Liquidtation>(liquidation);
            liquidationUpdate.LiquidationId= id;
            var respone = await _liquidationService.UpdateLiquidation(liquidationUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
