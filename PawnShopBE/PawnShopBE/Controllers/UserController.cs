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
            var respone = await _userService.sendEmail(email);
            if (respone)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(UserDTO request)
        {
            var user = _mapper.Map<User>(request);
            var response = await _userService.CreateUser(user);

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> getUserList(int numPage)
        {
            var userList = await _userService.GetAllUsers(numPage);
            if (userList == null)
            {
                return NotFound();
            }
            return Ok(userList);
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

            if (request != null)
            {
                var user = _mapper.Map<User>(request);
                var response = await _userService.UpdateUser(user);
                if (response)
                {
                    return Ok(response);
                }
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteUser/{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isUserCreated = await _userService.DeleteUser(userId);

            if (isUserCreated)
            {
                return Ok(isUserCreated);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
