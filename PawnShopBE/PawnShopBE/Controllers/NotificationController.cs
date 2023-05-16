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
        private IContractService _contractService;
        private IMapper _mapper;
        private INotificationService _notificationService;

        public NotificationController(IContractService contractService, IMapper mapper, INotificationService notificationService)
        {
            _contractService = contractService;
            _mapper = mapper;
            _notificationService = notificationService;
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
        [HttpPut("updateNotification/{notificationId}")]
        public async Task<IActionResult> readNotification(int notificationId, bool isRead)
        {
            var result = await _notificationService.UpdateNotification(notificationId, isRead);
            return (result) ? Ok(result) : BadRequest(result);
        }
    }
}
