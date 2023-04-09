using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IMoneyService
    {
        Task<bool> CreateMoney(Money money);

        Task<IEnumerable<Money>> GetAllMoney(int num, int branchId);
    }
}
