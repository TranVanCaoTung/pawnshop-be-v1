using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        public AuthenticationService(DbContextClass context, IOptionsMonitor<Appsetting> optionsMonitor)
        {
            _context = context;
            _appsetting = optionsMonitor.CurrentValue;
        }
        public async Task<IEnumerable<RefeshToken>> getAllToken()
        {
            var result=await _context.Set<RefeshToken>().ToListAsync();
            return result;
        }
        public async Task<TokenModel> GenerateToken(Admin admin)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appsetting.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
                SecurityAlgorithms.HmacSha512Signature)
            };
            //create Token
            var token = jwtToken.CreateToken(tokenDescription);
            var accessToken = jwtToken.WriteToken(token);
            var resfulToken = GenerateRefeshToken();

            //save data
            var refeshTokenEntity = new RefeshToken
            {
                Id = Guid.NewGuid(),
                JwtID = token.Id,
                Token = resfulToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
            };

            await _context.AddAsync(refeshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefeshToken = resfulToken
            };
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

        //Renew AccessToken
        public async Task<ApiRespone> RenewToken(TokenModel tokenModel)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appsetting.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
                ClockSkew = TimeSpan.Zero,

                //ko check Token hết hạn
                ValidateLifetime = false
            };

            try
            {
                //check 1: Accesstoken valid format
                var tokenInVerification = jwtToken.ValidateToken(tokenModel.AccessToken,
                    tokenValidateParam, out var validatedToken);

                //check 2: check thuat toan
                if (validatedToken is JwtSecurityToken jwtSecurity)
                {
                    var result = jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Response(false, "InvalidToken", null);
                    }
                }

                //check 3 check time expire
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(
                    x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Response(false, "Access token has not yet expired", null);
                }


                // check 4: refeshToken exist in db
                var storedToken = _context.RefeshTokens.FirstOrDefault(x => x.Token
                == tokenModel.RefeshToken);
                if (storedToken == null)
                {
                    return Response(false, "Refesh does not exits", null);
                }

                //check 5: refeshToken is used/revoked
                if (storedToken.IsUsed)
                {
                    return Response(false, "RefeshToken has Used", null);
                }
                if (storedToken.IsRevoked)
                {
                    return Response(false, "RefeshToken has Revoked", null);
                }

                //check 6: Access Token Id == jwtId in RefeshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type
                == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtID != jti)
                {
                    return Response(false, "Token doesn't match", null);
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new Token
                var admin = await _context.Admin.SingleOrDefaultAsync(ad => ad.UserName =="Admin");
                var token = await GenerateToken(admin);

                return Response(true, "Renew Token Access", token);
            }
            catch
            {
                return Response(false, "Something Went Wrong", null);
            }
        }
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
