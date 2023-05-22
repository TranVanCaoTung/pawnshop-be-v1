using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Org.BouncyCastle.Utilities;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customer, IMapper mapper)
        {
            _customer = customer;
            _mapper = mapper;
        }
        [HttpGet("getRelative/{id}")]
        public async Task<IActionResult> getCustomerRelative(Guid id)
        {
            var respone = await _customer.getRelative(id);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }

        [HttpPost("createRelative/{id}")]
        public async Task<IActionResult> createCustomerRelative(Guid id, Relative_Job_DependentDTO customer)
        {
            var respone = await _customer.createRelative(id, customer);
            return (respone) ? Ok(respone) : BadRequest(respone);
        }

        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
            //get Kyc id
            customer.KycId = await _customer.createKyc(customer);
            //create Customer
            var customerMap = _mapper.Map<Customer>(customer);
            var respone = await _customer.CreateCustomer(customerMap);
            return (respone) ? Ok(respone): BadRequest(respone);
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllCustomers(int numPage)
        {
            var listCustomer = await _customer.GetAllCustomer(numPage);
            var customersActive = from c in listCustomer where c.Status == (int)CustomerConst.ACTIVE select c;
            
            var response = _mapper.Map<IEnumerable<DisplayCustomer>>(customersActive);
            return (response != null) ? Ok(response.OrderByDescending(x => x.CustomerId)) : NotFound(response);          
        }
        [HttpGet("getAllBlackList/{numPage}")]
        public async Task<IActionResult> GetAllCustomersBlackList(int numPage)
        {
            var listCustomer = await _customer.GetAllCustomer(numPage);
            var customerBlackList = from c in listCustomer where c.Status == (int)CustomerConst.BLACKLIST select c;
            var response = _mapper.Map<IEnumerable<DisplayCustomer>>(customerBlackList);
            return (response != null) ? Ok(response.OrderByDescending(x => x.CustomerId)) : NotFound(response);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customer.GetCustomerById(id);
            return (customer == null) ? NotFound(customer) : Ok(customer);
        }

        [HttpDelete("deleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var listCustomer = await _customer.DeleteCustomer(id);
            return (!listCustomer) ? BadRequest(listCustomer) : Ok(listCustomer); 
        }

        [HttpPut("updateCustomer")]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            var respone = await _customer.UpdateCustomer(customer);
            return (respone) ? Ok(respone) : BadRequest(respone);
        }

        [HttpGet("getByCCCD/{cccd}")]
        public async Task<IActionResult> GetCustomerByCCCD(string cccd)
        {
            var customer = await _customer.getCustomerByCCCD(cccd);
            return (customer == null) ? NotFound(customer) : Ok(customer);
        }
    }
}
