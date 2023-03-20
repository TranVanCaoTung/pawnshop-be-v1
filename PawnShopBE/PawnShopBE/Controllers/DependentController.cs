using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/dependentPeople")]
    [ApiController]
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
        private Validation<DependentPeopleDTO> _validation=new Validation<DependentPeopleDTO>();
       
        [HttpPost("createDependentPeople")]
        public async Task<IActionResult> CreateDependent(DependentPeopleDTO dependent)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(dependent);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var dependentMapper  = _mapper.Map<DependentPeople>(dependent);
            var respone = await _dependentService.CreateDependent(dependentMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteDependentPeople/{id}")]
        public async Task<IActionResult> DeleteDependent(Guid id)
        {
            var respone = await _dependentService.DeleteDependent(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateDependentPeople/{id}")]
        public async Task<IActionResult> UpdateDependent(Guid id,DependentPeopleDTO dependent)
        {
            var dependentMapper=_mapper.Map<DependentPeople>(dependent);
            dependentMapper.DependentPeopleId = id;
            var respone = await _dependentService.UpdateDependent(dependentMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
