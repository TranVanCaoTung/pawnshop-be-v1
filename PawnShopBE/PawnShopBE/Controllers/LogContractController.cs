using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/logContract")]
    [ApiController]
    [Authorize]
    public class LogContractController : ControllerBase
    {
        private readonly ILogContractService _logContractService;
        public LogContractController(ILogContractService logContractService)
        {
            _logContractService = logContractService;
        }

        [HttpGet("all/{numPage}")]
        public async Task<IActionResult> GetAllLogContracts( int numPage)
        {
            var logContracts = await _logContractService.GetLogContracts(numPage);

            return (logContracts != null) ? Ok(logContracts) : BadRequest();
        }

        [HttpGet("logContractById/{contractId}")]
        public async Task<IActionResult> GetLogContractByContractId(int contractId)
        {
            var logContract = await _logContractService.LogContractsByContractId(contractId);

            return (logContract != null) ? Ok(logContract) : BadRequest();
        }

        [HttpGet("logContractByBranchId/{branchId}")]
        public async Task<IActionResult> GetLogContractByBranchId(int branchId)
        {
            var logContract = await _logContractService.LogContractsByBranchId(branchId);

            return (logContract != null) ? Ok(logContract) : BadRequest();
        }
    }
}
