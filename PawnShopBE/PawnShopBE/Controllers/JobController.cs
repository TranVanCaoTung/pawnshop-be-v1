using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobController(IJobService jobService, IMapper mapper) 
        { 
        _jobService=jobService;
            _mapper=mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllJob() {
            var respone =await _jobService.GetJob();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        private Validation<JobDTO> _validation=new Validation<JobDTO>();
        
        [HttpPost("createJob")]
        public async Task<IActionResult> CreateJob(JobDTO job)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(job);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var jobMapper = _mapper.Map<Job>(job);
            var respone = await _jobService.CreateJob(jobMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var respone = await _jobService.DeleteJob(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateJob/{id}")]
        public async Task<IActionResult> UpdateJob(int id,JobDTO job)
        {
            var jobUpdate=_mapper.Map<Job>(job);
            jobUpdate.JobId = id;
            var respone = await _jobService.UpdateJob(jobUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
