using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/ramsom")]
    [ApiController]
    [Authorize]
    public class RansomController : ControllerBase
    {
        private readonly IRansomService _ranSomeservices;
        private readonly IMapper _mapper;

        public RansomController(IRansomService ransomService, IMapper mapper)
        {
            _ranSomeservices = ransomService;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllRansom()
        {
            var respone = await _ranSomeservices.GetRansom();
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }
        [HttpGet("ransombyid/{contractId}")]
        public async Task<IActionResult> ransombyContractId( int contractId)
        {
            var response = await _ranSomeservices.GetRansomByContractId(contractId);
            return (response!= null) ? Ok(response) : BadRequest(response);
        }

        [HttpPut("saveransom/{ransomId}")]
        public async Task<IActionResult> SaveRansom(int ransomId, string proofImg)
        {
            var response = await _ranSomeservices.SaveRansom(ransomId, proofImg);
            return (response) ? Ok(response) : BadRequest(response);
        }
    }
}
