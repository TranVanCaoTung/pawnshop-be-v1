using AutoMapper;
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
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        [HttpGet("ransombyid/{contractId}")]
        public async Task<IActionResult> ransombyContractId(int contractId)
        {

            var response = await _ranSomeservices.GetRansomByContractId(contractId);
            if (response!= null)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPut("saveransom/{ransomId}")]
        public async Task<IActionResult> SaveRansom(int ransomId)
        {
            var response = await _ranSomeservices.SaveRansom(ransomId);
            if (response != null)
            {
                return Ok("Save Success");
            }

            return BadRequest();
        }
    }
}
