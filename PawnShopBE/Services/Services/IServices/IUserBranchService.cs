using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IUserBranchService
    {
        Task<bool> CreateUserBranch(UserBranch userBranch);
        Task<bool> UpdateUserBranch(UserBranch userBranch);
        Task<UserBranch> GetUserBranchById(int id);
        Task<IEnumerable<UserBranch>> GetAllUserBranches();

    }
}
