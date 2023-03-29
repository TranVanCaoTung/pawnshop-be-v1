using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/attribute")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        private readonly IMapper _mapper;
        public AttributeController(IAttributeService attributeService, IMapper mapper)
        {
            _attributeService = attributeService;
            _mapper = mapper;
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetAttributesById(int id)
        {
            var attributes = await _attributeService.GetAttributesByPawnableId(id);

            if (attributes != null)
            {
                return Ok(attributes);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
