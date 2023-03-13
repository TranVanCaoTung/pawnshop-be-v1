using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllPackages(int num);
        Task<Package> GetPackageById(int packageId, int interestRecommend);
    }
}
