
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;
        private readonly IMapper _mapper;

        public KycController(IKycService kycService, IMapper mapper) 
        {
        _kycService=kycService;
            _mapper=mapper;
        }

        [HttpGet("kyc")]
        public async Task<IActionResult> GetAll() {
        var listKyc=await _kycService.GetAllKyc();
            if (listKyc == null)
                return BadRequest();
            return Ok(listKyc);
        }

        [HttpPost("kyc")]
        public async Task<IActionResult> CreateKyc(KycDTO kyc)
        {
            if (kyc != null)
            {
                var kycCreate = _mapper.Map<Kyc>(kyc);
                var respone= await _kycService.CreateKyc(kycCreate);
                if(respone!= null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

    }
}
