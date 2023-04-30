using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/ledger")]
    [ApiController]
    [Authorize]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public LedgerController(ILedgerService ledgerService, IMapper mapper) 
        { 
        _ledgerService=ledgerService;
            _mapper=mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllLedger() {
            var respone =await _ledgerService.GetLedger();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpGet("getbyBranchId/{branchId}/{year}")]
        public async Task<IActionResult> GetByBranchId(int branchId, int year)
        {
            var respone = await _ledgerService.GetLedgersByBranchId(branchId, year);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpGet("yearsOfLeger")]
        public async Task<IActionResult> GetYearsOfLeger()
        {
            var respone = await _ledgerService.GetYearsOfLedger();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateLedger")]
        public async Task<IActionResult> UpdateLedger( LedgerDTO ledger)
        {
            var ledgerUpdate=_mapper.Map<Ledger>(ledger);
            var respone = await _ledgerService.UpdateLedger(ledgerUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
