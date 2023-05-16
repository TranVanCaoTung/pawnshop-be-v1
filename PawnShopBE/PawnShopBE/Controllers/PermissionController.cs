
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/permission")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _perService;
        private readonly IMapper _mapper;

        public PermissionController(IPermissionService permission, IMapper mapper)
        {
            _perService = permission;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var listKyc = await _perService.GetPermission();
            return (listKyc == null) ? BadRequest() : Ok(listKyc);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Permission per)
        {
            var respone = await _perService.CreatePermission(per);
            return (respone) ? Ok(respone) : BadRequest("Permission Is Exists");
        }
        [HttpPut("savepermission")]
        public async Task<IActionResult> SavePermission(IEnumerable<DisplayPermission> user)
        {
            if (user != null)
            {
                await _perService.SavePermission(user);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("showpermission/{userId}")]
        public async Task<IActionResult> ShowPermission(Guid userId)
        {
            var respone = await _perService.ShowPermission(userId);
            return (respone != null) ? Ok(respone) : BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var respone = await _perService.DeletePermission(id);
            return (respone != null) ? Ok(respone): BadRequest();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePermission(Permission per)
        {
            var respone = await _perService.UpdatePermission(per);
            return (respone != null) ? Ok(respone) : BadRequest(respone);
        }
    }
}
