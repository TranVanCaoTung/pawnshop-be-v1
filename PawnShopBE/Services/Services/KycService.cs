using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class KycService : IKycService
    {
        private readonly IUnitOfWork _unit;

        public KycService(IUnitOfWork unitOfWork) {
            _unit = unitOfWork;
        }
        public async Task<Kyc> CreateKyc(Kyc kyc)
        {
            await _unit.Kycs.Add(kyc);
            var result = _unit.Save();
            if (result > 0)
                return kyc;

            return null;
        }

        public async Task<bool> DeleteKyc(int idKyc)
        {
            var kyc =await _unit.Kycs.GetById(idKyc);
            if (kyc != null)
            {
                _unit.Kycs.Delete(kyc);
                var result = _unit.Save();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public async Task<IEnumerable<Kyc>> GetAllKyc()
        {
            var kycList = await _unit.Kycs.GetAll();
            return kycList;
        }

        public async Task<Kyc?> GetKycById(int kycId)
        {
            var kyc = await _unit.Kycs.GetById(kycId);
            return (kyc != null) ? kyc : null;             
        }

        public async Task<bool> UpdateKyc(Kyc kyc)
        {
            if (kyc != null)
            {
                var kycUpdate = await _unit.Kycs.GetById(kyc.KycId);
                if (kycUpdate != null)
                {
                    _unit.Kycs.Update(kycUpdate);
                    var result = _unit.Save();
                    if (result > 0)
                        return true;
                }
            }
            return false;
        }
    }
}
