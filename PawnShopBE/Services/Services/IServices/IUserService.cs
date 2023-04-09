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
        Task<bool> CreateUser(User user);
        Task<bool> CreateAdmin(Admin admin);
        Task<IEnumerable<User>> GetAllUsers(int num);

        Task<User> GetUserById(Guid userId);

        Task<bool> UpdateUser(User user);

        Task<bool> DeleteUser(Guid userId);
        Task<bool> sendEmail(string email);
    }
}
