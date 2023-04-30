using Microsoft.AspNetCore.Http;
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

namespace PawnShopBE.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbContextClass _context;
        private IAuthentication _authen;
        private IMapper _mapper;
        public AuthenticationController(DbContextClass context, IAuthentication authentication, IMapper mapper)
        {
            _context = context;
            _authen = authentication;
            _mapper = mapper;
        }
        //[HttpPost("renewToken")]
        //public async Task<IActionResult> RenewToken(TokenModel tokenmodel)
        //{
        //    var token = tokenmodel;
        //    if (token != null)
        //    {
        //        var respone = await _authen.RenewToken(token);
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}
        private string name { get; set; }
        private string email { get; set; }
        private string id { get; set; }
        private string branchid { get; set; }

        [HttpPost("decrypttoken")]
        public async Task<IActionResult> DecryptToken(TokenModel tokenmodel)
        {
            var token = tokenmodel.AccessToken;
            if (token != null)
            {
                var readToken = _authen.EncrypToken(token);
                var respone = readToken.Claims;
                foreach(var x in respone)
                {
                   switch (x.Type)
                    {
                        case "Email":
                            email=x.Value;
                            break;
                        case "UserId":
                            id = x.Value;
                            break;
                        case "Name":
                            name = x.Value;
                            break;
                        case "BranchId":
                            branchid= x.Value;
                            break;
                    }
                }
                return Ok(new
                {
                    Name = name,
                    Email=email,
                    BranchId=branchid,
                    UserId=id
                }) ;
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
                    var userRepsonse = _mapper.Map<UserRepsonse>(user);
                    // cấp token
                    var token = await _authen.GenerateToken(user, null) ;
                    if (token != null)
                    {
                        return Ok(new
                        {
                            Token = token
                        });
                    }
                }
            }
            
            var admin = _context.Admin.SingleOrDefault(p => p.UserName == login.userName);
            if (admin != null)
            {
                bool isValidAdminPassword = BCrypt.Net.BCrypt.Verify(login.password, admin.Password);
                if (isValidAdminPassword)
                {
                    // cấp token
                    var token = await _authen.GenerateToken(null,admin);
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
