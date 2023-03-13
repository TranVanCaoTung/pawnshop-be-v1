using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PackageService : IPackageService
    {
        public IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Package>> GetAllPackages(int num)
        {
            var packageList = await _unitOfWork.Packages.GetAll();
            if (num == 0)
            {
                return packageList;
            }
            var result= await _unitOfWork.Packages.TakePage(num,packageList);
            return result;
        }

        public async Task<Package> GetPackageById(int packageId, int interestRecommend)
        {
            if (packageId != null)
            {
                var package = await _unitOfWork.Packages.GetById(packageId);
                if (interestRecommend != 0)
                {
                    package.PackageInterest = interestRecommend;
                }
                if (package != null)
                {
                    return package;
                }
            }
            return null;
        }
    }
}
