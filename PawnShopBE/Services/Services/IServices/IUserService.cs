using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserDTO user);
        Task<IEnumerable<User>> GetAllUsers(int num);
        Task<DisplayUser> GetUserById(Guid userId);
        Task<bool> UpdateUser(User user, int branchId);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> RecoveryPassword(string email);
        Task<User> GetAdmin(int role);
        Task<bool> ChangePassword(Guid userId, string oldPwd, string newPwd);
    }
}
