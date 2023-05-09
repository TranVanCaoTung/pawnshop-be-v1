using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IPermissionService
    {
        Task<bool> CreatePermission(Permission permission);
        Task<IEnumerable<Permission>> GetPermission();
        Task<bool> UpdatePermission(Permission permission);
        Task<bool> DeletePermission(int perId);
        Task<IEnumerable<DisplayPermission>> ShowPermission(Guid userId);
        Task SavePermission(IEnumerable<DisplayPermission> listPermission);

    }
}
