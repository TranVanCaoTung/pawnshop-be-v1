using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IRoleService
    {
        Task<bool> CreateRole(Role role);

        Task<bool> DeleteRole(int roleId);
        Task<Role> GetRole(int roleId);
    }
}
