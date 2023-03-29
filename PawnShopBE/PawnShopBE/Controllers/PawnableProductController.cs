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
    [Route("api/v1/pawnableProduct")]
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
        
        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllPawnableProducts(int numPage)
        {
            var respone = await _pawnableProductService.GetAllPawnableProducts(numPage);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpGet("getPawnAbleProductById/{id}")]
        public async Task<IActionResult> PawnAbleProductById(int id)
        {
            var respone = await _pawnableProductService.GetPawnableProductById(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("createPawnable")]
        public async Task<IActionResult> CreatePawnableProduct(PawnableDTO pawnableDTO)
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

        [HttpPut("updatePawnableProduct/{pawnableProductId}")]
        public async Task<IActionResult> UpdatePawnableProduct(int pawnableProductId,  PawnableDTO request)
        {                
                var pawnableProduct = _mapper.Map<PawnableProduct>(request);
                pawnableProduct.PawnableProductId = pawnableProductId;
                var response = await _pawnableProductService.UpdatePawnableProduct(pawnableProduct);
                if (response)
                {
                    return Ok(response);
                }           
            return BadRequest();
        }


    }
}
