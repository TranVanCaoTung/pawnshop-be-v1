using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/branch")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch(BranchDTO request)
        {

            var branch = _mapper.Map<Branch>(request);
            var response = await _branchService.CreateBranch(branch);

            return (response) ? Ok(response) : BadRequest();
        }
        [HttpGet("getDetailById/{branchId}")]
        public async Task<IActionResult> GetBranchDetail(int branchId)
        {
            var branchDetail = await _branchService.getDisplayBranchDetail(branchId);
            return (branchDetail == null) ? NotFound(branchDetail) : Ok(branchDetail);

        }

        [HttpGet("getDetailYearById/{branchId}/{year}")]
        public async Task<IActionResult> GetBranchDetailYear(int branchId, int year)
        {
            var branchDetail = await _branchService.getDisplayBranchYearDetail(branchId, year);
            return (branchDetail == null) ? NotFound(branchDetail) : Ok(branchDetail);

        }
        [HttpGet("getChain")]
        public async Task<IActionResult> GetBranchChain()
        {
            var displayBranch = await _branchService.getDisplayBranch();
            return (displayBranch == null) ? NotFound() : Ok(displayBranch);
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetBranchList(int numPage)
        {
            var branchList = await _branchService.GetAllBranch(numPage);
            return (branchList == null) ? NotFound(branchList) : Ok(branchList);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _branchService.GetBranchById(id);
            return (branch != null) ? Ok(branch) : BadRequest();
        }

        [HttpPut("updateBranch/{id}")]
        public async Task<IActionResult> UpdateBranch(int id, BranchRequest request)
        {
            if (id != null)
            {
                // var branch = _mapper.Map<Branch>(request);
                var response = await _branchService.UpdateBranch(id, request);
                if (response)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }
        [HttpDelete("deleteBranch/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var isBranchCreated = await _branchService.DeleteBranch(id);

            if (isBranchCreated)
            {
                return Ok(isBranchCreated);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
