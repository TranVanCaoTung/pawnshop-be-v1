using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PawnShopBE.Core.Data;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Services.Services
{
    public class AuthenticationService : IAuthentication
    {
        private readonly DbContextClass _context;
        private readonly Appsetting _appsetting;
        //private UserManager<User> _userManager;
        //private SignInManager<User> _signInManager;

       
        public AuthenticationService(DbContextClass context, IOptionsMonitor<Appsetting> optionsMonitor)
            //,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _appsetting = optionsMonitor.CurrentValue;
            //_userManager= userManager;
            //_signInManager= signInManager;
        }
        public async Task<bool> Login(Login user)
        {
            //var userName = await _userManager.FindByNameAsync(user.userName);
            //if(userName == null) return false;

            //var result = await _signInManager.PasswordSignInAsync(userName,user.password,user.remember,true);
            //if(!result.Succeeded) return false;
            return true;
        }
        public async Task<IEnumerable<RefeshToken>> getAllToken()
        {
            var result=await _context.Set<RefeshToken>().ToListAsync();
            return result;
        }
        public async Task<TokenModel> GenerateToken(User user,Admin admin)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appsetting.SecretKey);
            SecurityTokenDescriptor tokenDescription;
            if (user != null)
            {
                //token for user
               tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Email",user.Email),
                    new Claim("Name",user.FullName),
                    new Claim("BranchId",user.BranchId.ToString()),
                    new Claim("UserId",user.UserId.ToString())
                }),

                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                
            }
            else
            {
                var branchList = await _context.Branch.ToListAsync();
                var firstBranch = branchList.FirstOrDefault();
                //token for admin
                tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Email",admin.Email),
                    new Claim("Name",admin.UserName),
                    new Claim("BranchId",firstBranch.BranchId.ToString()),
                    new Claim("UserId",admin.Id.ToString())
                }),

                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
                    SecurityAlgorithms.HmacSha512Signature)
                };
               
            }
            //create Token
            var token = jwtToken.CreateToken(tokenDescription);
            var accessToken = jwtToken.WriteToken(token);
            return new TokenModel
            {
                AccessToken = accessToken
            };
            //var resfulToken = GenerateRefeshToken();
            //save data
            //var refeshTokenEntity = new RefeshToken
            //{
            //    Id = Guid.NewGuid(),
            //    JwtID = token.Id,
            //    Token = resfulToken,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    IssuedAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddHours(2),
            //};

            //await _context.AddAsync(refeshTokenEntity);
            //await _context.SaveChangesAsync();
            //accessToken ="Bearer "+accessToken;
        }
        private string GenerateRefeshToken()
        {
            var random = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                string token = Convert.ToBase64String(random);
                return token;
            }
        }
        //giải mã token
        public ClaimsPrincipal EncrypToken(string token)
        {
            var tokenHandler =new JwtSecurityTokenHandler();
            //Decode JWT
            var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsetting.SecretKey)),
                ValidateIssuer=false,
                ValidateAudience=false,
                ClockSkew=TimeSpan.Zero
            },out SecurityToken validatedToken);
            //Access the claim 
            return claims;
        }

        #region Renew AccessToken
        //public async Task<ApiRespone> RenewToken(TokenModel tokenModel)
        //{
        //    var jwtToken = new JwtSecurityTokenHandler();
        //    var secretKeyByte = Encoding.UTF8.GetBytes(_appsetting.SecretKey);
        //    var tokenValidateParam = new TokenValidationParameters
        //    {
        //        // tự cấp token
        //        ValidateIssuer = false,
        //        ValidateAudience = false,

        //        //ký vào token
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
        //        ClockSkew = TimeSpan.Zero,

        //        //ko check Token hết hạn
        //        ValidateLifetime = false
        //    };

        //    try
        //    {
        //        //check 1: Accesstoken valid format
        //        var tokenInVerification = jwtToken.ValidateToken(tokenModel.AccessToken,
        //            tokenValidateParam, out var validatedToken);

        //        //check 2: check thuat toan
        //        if (validatedToken is JwtSecurityToken jwtSecurity)
        //        {
        //            var result = jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
        //                StringComparison.InvariantCultureIgnoreCase);
        //            if (!result)
        //            {
        //                return Response(false, "InvalidToken", null);
        //            }
        //        }

        //        //check 3 check time expire
        //        var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(
        //            x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        //        var expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
        //        if (expireDate > DateTime.UtcNow)
        //        {
        //            return Response(false, "Access token has not yet expired", null);
        //        }


        //        // check 4: refeshToken exist in db
        //        var storedToken = _context.RefeshTokens.FirstOrDefault(x => x.Token
        //        == tokenModel.RefeshToken);
        //        if (storedToken == null)
        //        {
        //            return Response(false, "Refesh does not exits", null);
        //        }

        //        //check 5: refeshToken is used/revoked
        //        if (storedToken.IsUsed)
        //        {
        //            return Response(false, "RefeshToken has Used", null);
        //        }
        //        if (storedToken.IsRevoked)
        //        {
        //            return Response(false, "RefeshToken has Revoked", null);
        //        }

        //        //check 6: Access Token Id == jwtId in RefeshToken
        //        var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type
        //        == JwtRegisteredClaimNames.Jti).Value;
        //        if (storedToken.JwtID != jti)
        //        {
        //            return Response(false, "Token doesn't match", null);
        //        }

        //        //Update token is used
        //        storedToken.IsRevoked = true;
        //        storedToken.IsUsed = true;
        //        _context.Update(storedToken);
        //        await _context.SaveChangesAsync();

        //        var user=modelt
        //        //create new Token
        //        var token = await GenerateToken();

        //        return Response(true, "Renew Token Access", token);
        //    }
        //    catch
        //    {
        //        return Response(false, "Something Went Wrong", null);
        //    }
        //}
        #endregion Renew AccessToken
        private DateTime ConverUrnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
        //Respon Authentication, Status
        private ApiRespone Response(bool success, string message, object? data)
        {
            return new ApiRespone
            {
                Success = success,
                Message = message,
                Data = data
            };
        }

    }
}
