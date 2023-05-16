using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private IUserBranchService _userBranchService;
        public UserController(IUserService userService, IMapper mapper, IUserBranchService userBranchService)
        {
            _userService = userService;
            _mapper = mapper;
            _userBranchService = userBranchService;
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

        [HttpGet("getAll/{numPage}/{branchId}")]
        public async Task<IActionResult> getUserList(int numPage, int branchId)
        {
            var userList = await _userService.GetAllUsers(numPage);
            var userListByBranch = new List<User>();
            foreach (var user in userList)
            {
                if (user.RoleId == (int)RoleConst.ADMIN)
                {
                    continue;
                }
                var userBranchList = await _userBranchService.GetUserBranchByBranchId(branchId);
                foreach(var userBranch in userBranchList)
                {
                    if (user.UserId == userBranch.UserId)
                    {
                        userListByBranch.Add(user);
                    }
                }
            }
            return (userListByBranch != null) ? Ok(userListByBranch) : NotFound();
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
        //[HttpPut("updatePassword")]
        //public async Task<IActionResult> UpdatePassword(string newPwd)
        //{
        //    var response = await _userService.UpdateUser(user, request.BranchId);
        //    return (response) ? Ok(response) : BadRequest(response);
        //}

        [HttpDelete("deleteUser/{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isUserCreated = await _userService.DeleteUser(userId);
            return (isUserCreated) ? Ok(isUserCreated) : BadRequest();
        }

    }
}
