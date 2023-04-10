using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/notification")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public NotificationController(IContractService contractService, IMapper mapper)
        {
            _contractService = contractService;
            _mapper = mapper;
        }
        [HttpGet("notificationList/{branchId}")]
        public async Task<IActionResult> getListContractToday(int branchId)
        {
            var result =  await _contractService.NotificationList(branchId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
