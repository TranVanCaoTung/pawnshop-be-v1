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

        public async Task<bool> CreatePackage(Package package)
        {
            if (package == null) return false;
            await _unitOfWork.Packages.Add(package);
            var result = _unitOfWork.Save();

            return (result > 0) ? true : false;
        }

        public async Task<IEnumerable<Package>> GetAllPackages(int num)
        {
            var packageList = await _unitOfWork.Packages.GetAll();
            if (num == 0)
            {
                return packageList;
            }
            var result = await _unitOfWork.Packages.TakePage(num, packageList);
            return result;
        }

        public async Task<Package> GetPackageById(int packageId)
        {
            if (packageId != null)
            {
                var package = await _unitOfWork.Packages.GetById(packageId);
                if (package != null)
                {
                    return package;
                }
            }
            return null;
        }

        public async Task<bool> UpdatePackage(Package package)
        {
            var packageUpdate = await _unitOfWork.Packages.GetById(package.PackageId);
            if (packageUpdate != null)
            {
                packageUpdate.PackageName = package.PackageName;
                packageUpdate.PackageInterest = package.PackageInterest;
                packageUpdate.Day = package.Day;
                packageUpdate.PaymentPeriod = package.PaymentPeriod;
                packageUpdate.Limitation = package.Limitation;
                packageUpdate.PunishDay1 = package.PunishDay1;
                packageUpdate.PunishDay2 = package.PunishDay2;
                packageUpdate.LiquitationDay = package.LiquitationDay;
                packageUpdate.InterestDiaryPenalty = package.InterestDiaryPenalty;
                packageUpdate.RansomPenalty = package.RansomPenalty;
                var result = _unitOfWork.Save();
                if (result > 0)
                    return true;
            }
            return false;
        }
    }
}
