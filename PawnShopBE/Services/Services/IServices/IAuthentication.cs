using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Data;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IAuthentication
    {
        Task<TokenModel> GenerateToken();
        Task<ApiRespone> RenewToken(TokenModel tokenModel);
        Task<IEnumerable<RefeshToken>> getAllToken();
        Task<bool> Login(Login user);
    }
}
