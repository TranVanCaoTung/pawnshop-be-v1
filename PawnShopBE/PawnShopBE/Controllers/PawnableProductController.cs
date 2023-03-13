using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;
using System.Collections.Generic;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PawnableProductController : ControllerBase
    {

        private readonly IPawnableProductService _pawnableProductService;
        private readonly IAttributeService _attributeService;
        private readonly IMapper _mapper;
        public PawnableProductController(IPawnableProductService pawnableProductService,
                                         IAttributeService attributeService,
                                         IMapper mapper)
        {
            _pawnableProductService = pawnableProductService;
            _attributeService = attributeService;
            _mapper = mapper;
        }
        
        [HttpGet("pawnable/{numPage}")]
        public async Task<IActionResult> GetAllPawnable(int numPage)
        {
            var respone = await _pawnableProductService.GetAllPawnableProducts(numPage);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("pawnable")]
        public async Task<IActionResult> CreatePawnable(PawnableDTO pawnableDTO)
        {
            var attribute = _mapper.Map<ICollection<Attribute>>(pawnableDTO.AttributeDTOs);
            var pawnable = _mapper.Map<PawnableProduct>(pawnableDTO);
            pawnable.Attributes = attribute;
            var respone = await _pawnableProductService.CreatePawnableProduct(pawnable);

            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("pawnable/{id}")]
        public async Task<IActionResult> UpdatePawnableProduct(int id, PawnableDTO request)
        {
                     
                var pawnableProduct = _mapper.Map<PawnableProduct>(request);
                pawnableProduct.PawnableProductId = id;

                var response = await _pawnableProductService.UpdatePawnableProduct(pawnableProduct);
                if (response)
                {
                    return Ok(response);
                }
            
            return BadRequest();
        }


    }
}
