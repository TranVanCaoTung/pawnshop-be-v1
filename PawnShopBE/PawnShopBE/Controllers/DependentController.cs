using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/dependentPeople")]
    [ApiController]
    [Authorize]
    public class DependentController : ControllerBase
    {
        private readonly IDependentService _dependentService;
        private readonly IMapper _mapper;

        public DependentController(IDependentService dependentService, IMapper mapper) 
        { 
        _dependentService=dependentService;
            _mapper=mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllDependent() {
            var respone =await _dependentService.GetDependent();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
       
        [HttpPost("createDependentPeople")]
        public async Task<IActionResult> CreateDependent( DependentPeopleDTO dependent)
        {
            
            var dependentMapper  = _mapper.Map<DependentPeople>(dependent);
            var respone = await _dependentService.CreateDependent(dependentMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteDependentPeople/{id}")]
        public async Task<IActionResult> DeleteDependent( Guid id)
        {
            var respone = await _dependentService.DeleteDependent(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateDependentPeople")]
        public async Task<IActionResult> UpdateDependent( DependentPeopleDTO dependent)
        {
            var dependentMapper=_mapper.Map<DependentPeople>(dependent);
            var respone = await _dependentService.UpdateDependent(dependentMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
