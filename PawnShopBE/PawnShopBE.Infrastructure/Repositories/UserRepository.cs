using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContextClass dbContext) : base(dbContext)
        {

        }

        public async Task<User> GetUserByEmail(string email)
        {
            return _dbContext.User.FirstOrDefault(u => u.Email == email);
        }
    }
}
