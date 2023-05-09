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
        Task<User> GetUserById(Guid userId);
        Task<bool> UpdateUser(User user, int branchId);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> SendEmail(string email);
        Task<User> GetAdmin(int role);
    }
}
