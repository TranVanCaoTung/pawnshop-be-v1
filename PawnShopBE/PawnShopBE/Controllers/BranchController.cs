using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CreateBranch( BranchDTO request)
        {
           
            var branch = _mapper.Map<Branch>(request);
            var response = await _branchService.CreateBranch(branch);

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("getDetailById/{id}")]

        public async Task<IActionResult> GetBranchDetail(int id)
        {
            var branchList = await _branchService.GetBranchById(id);
            var branchDetail = _mapper.Map<DisplayBranchDetail>(branchList);
            if (branchDetail != null)
            {
                branchDetail = await _branchService.getDisplayBranchDetail(branchDetail);
                return Ok(branchDetail);
            }
            return BadRequest();
        }
        [HttpGet("getChain")]
        public async Task<IActionResult> GetBranchChain()
        {
            var branchList = await _branchService.GetAllBranch(0);
            var displayBranch = _mapper.Map<IEnumerable<DisplayBranch>>(branchList);
            if (displayBranch != null)
            {
                displayBranch = await _branchService.getDisplayBranch(displayBranch);
                return Ok(displayBranch);
            }
            return NotFound();
        }
        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetBranchList( int numPage)
        {
            var branchList = await _branchService.GetAllBranch(numPage);
            if (branchList == null)
            {
                return NotFound();
            }
            return Ok(branchList);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetBranchById( int id)
        {
            var branch = await _branchService.GetBranchById(id);

            if (branch != null)
            {
                return Ok(branch);
            }
            else
            {
                return BadRequest();
            }
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

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var branch = await _branchService.GetBranchById(id);

            if (branch != null)
            {
                return Ok(branch);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
