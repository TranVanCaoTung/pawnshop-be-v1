using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RoleService : IRoleService
    {
        public IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateRole(Role role)
        {
            if (role != null)
            {
                
                await _unitOfWork.Roles.Add(role);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public Task<bool> DeleteRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetRole(int roleId)
        {
            if (roleId != null)
            {
                var role = await _unitOfWork.Roles.GetById(roleId);
                if (role != null)
                {
                    return role;
                }
            }
            return null;
        }
    }
}
