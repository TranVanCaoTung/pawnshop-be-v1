using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/customerRelative")]
    [ApiController]
    public class CustomerRelativeController : ControllerBase
    {
        private readonly ICustomerRelativeService _customerRelative;
        private readonly IMapper _mapper;

        public CustomerRelativeController(ICustomerRelativeService customerRelativeService, IMapper mapper) 
        { 
        _customerRelative=customerRelativeService;
            _mapper=mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllCustomerRelative() {
            var respone =await _customerRelative.GetCustomerRelative();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        private Validation<CustomerRelativeDTO> _validation=new Validation<CustomerRelativeDTO>();
       
    [HttpPost("createCustomerRelative")]
        public async Task<IActionResult> CreateCustomerRelative(CustomerRelativeDTO customerRelative)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(customerRelative);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var customerRelativeMapper = _mapper.Map<CustomerRelativeRelationship>(customerRelative);
            var respone = await _customerRelative.CreateCustomerRelative(customerRelativeMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteCustomerRelative/{id}")]
        public async Task<IActionResult> DeleteCustomerRelative(Guid id)
        {
            var respone = await _customerRelative.DeleteCustomerRelative(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateCustomerRelative")]
        public async Task<IActionResult> UpdateCustomerRelative(CustomerRelativeDTO customerRelative)
        {
            var customerRelativeMapper = _mapper.Map<CustomerRelativeRelationship>(customerRelative);
            var respone = await _customerRelative.UpdateCustomerRelative(customerRelativeMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
