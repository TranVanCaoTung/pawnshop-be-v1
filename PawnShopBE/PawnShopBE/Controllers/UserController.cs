using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("recoveryPassword/{email}")]
        public async Task<IActionResult> recoverPass(string email)
        {
            var respone = await _userService.SendEmail(email);
            return (respone) ? Ok(respone) : BadRequest(respone);
        }


        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(UserDTO request)
        {
            var response = await _userService.CreateUser(request);
            return (response) ? Ok(response) : BadRequest(response);
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> getUserList(int numPage)
        {
            var userList = await _userService.GetAllUsers(numPage);
            return (userList != null) ? Ok(userList) : NotFound();
        }

        [HttpGet("getUserById/{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            return (user != null) ? Ok(user) : BadRequest();
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO request)
        {
            var user = _mapper.Map<User>(request);
            var response = await _userService.UpdateUser(user, request.BranchId);
            return (response) ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("deleteUser/{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isUserCreated = await _userService.DeleteUser(userId);
            return (isUserCreated) ? Ok(isUserCreated) : BadRequest();
        }

    }
}
