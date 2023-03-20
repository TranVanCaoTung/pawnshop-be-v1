using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

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

        //[HttpPut("ransomeBeforeEndDate/{id}")]
        //public async Task<IActionResult> RansomeBeforeEndDate(PawnableDTO request)
        //{

        //    var pawnableProduct = _mapper.Map<PawnableProduct>(request);
        //    pawnableProduct.PawnableProductId = id;

        //    var response = await _ranSomeservices.(pawnableProduct);
        //    if (response)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest();
        //}
    }
}
