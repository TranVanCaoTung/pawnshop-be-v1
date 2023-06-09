﻿using Microsoft.AspNetCore.Http;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PawnShopBE.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Services.Services.IServices;
using PawnShopBE.Core.Const;
using AutoMapper;
using PawnShopBE.Core.Responses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using System.Text.Json;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbContextClass _context;
        private IAuthentication _authen;
        private IMapper _mapper;
        private IPermissionService _permissionService;
        private IUserService _userService;
        public AuthenticationController(DbContextClass context, IAuthentication authentication, IMapper mapper, IPermissionService permissionService, IUserService userService)
        {
            _context = context;
            _authen = authentication;
            _mapper = mapper;
            _permissionService = permissionService;
            _userService = userService;
        }
        [HttpPost("decrypttoken")]
        public async Task<IActionResult> DecryptToken(TokenModel tokenmodel)
        {
            var token = tokenmodel.AccessToken;
            if (token != null)
            {
                var readToken = _authen.EncrypToken(token);
                var respone = readToken.Claims;
                var branchIds = new List<int>();
                var userId = new Guid();
                foreach (var x in respone)
                {
                    switch (x.Type)
                    {
                        case "UserId":
                            userId = Guid.Parse(x.Value);
                            break;
                        case "BranchIds":
                            branchIds = x.Value.Split(',').Select(int.Parse).ToList();
                            break;
                    }
                }
                var userPermissions = await _permissionService.ShowPermission(userId);
                var user = await _userService.GetUserById(userId);
                user.UserPermission = (ICollection<Core.Display.DisplayPermission>)userPermissions;
                return Ok(new
                {
                    User = user,
                    BranchIds = branchIds,
                });
            }
            return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = _context.User.SingleOrDefault(p => p.UserName == login.userName);
            if (user != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(login.password, user.Password);
                if (isValidPassword)
                {
                    var userRepsonse = new UserRepsonse();

                    // Get Branch list
                    var userBranchs = _context.UserBranches.Where(u => u.UserId == user.UserId);
                    var listBranch = new List<int>();
                    foreach (var branch in userBranchs)
                    {
                        listBranch.Add(branch.BranchId);
                    }
                    userRepsonse.UserId = user.UserId;
                    userRepsonse.BranchIds = listBranch;
                    // cấp token
                    var token = await _authen.GenerateToken(userRepsonse);
                    if (token != null)
                    {
                        return Ok(new
                        {
                            Token = token
                        });
                    }
                }
            }
            return BadRequest(new
            {
                result = "Invalid UserName or Password"
            });
        }
        [HttpGet]
        public async Task<IActionResult> getAllToken()
        {
            var respone = await _authen.getAllToken();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }


    }
}
