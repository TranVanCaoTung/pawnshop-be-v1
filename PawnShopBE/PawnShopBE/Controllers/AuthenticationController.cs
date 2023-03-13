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

namespace PawnShopBE.Controllers
{
    [Route("api/authentication/login/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbContextClass _context;
        private IAuthentication _authen;
        public AuthenticationController(DbContextClass context,IAuthentication authentication) 
        {
            _context = context;
            _authen = authentication;
        }
        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenmodel)
        {
            var token = tokenmodel;
            if (token != null)
            {
               var respone = await _authen.RenewToken(token);
                return Ok(respone);
            }
            return BadRequest();
        }
        [HttpPost("create")]
        public async Task<IActionResult> Validate(Login login)
        {
            var user= _context.User.SingleOrDefault(p => p.UserName == login.userName &&
            p.Password== login.password && p.RoleId==(int)RoleConst.ADMIN);
            if(user == null)
            {
                return BadRequest(new
                {
                    result="Invalid UserName or Password"
                });
            }
            // cấp token
            var token = await _authen.GenerateToken(user);
            if (token != null)
            {
                return Ok(token);
            }
             return BadRequest();
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
