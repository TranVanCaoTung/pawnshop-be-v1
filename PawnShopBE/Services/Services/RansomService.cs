using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RansomService : IRansomService
    {
        public IUnitOfWork _unitOfWork;

        public RansomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateRansom(Contract contract)
        {
            if (contract != null)
            {
                var ransom = new Ransom();
                ransom.ContractId = contract.ContractId;
                ransom.Payment = contract.Loan + contract.TotalProfit;
                ransom.Penalty = 0;
                ransom.PaidMoney = 0;
                ransom.PaidDate = null;
                ransom.Status = (int) RansomConsts.ON_TIME;
                ransom.Description = null;
                ransom.ProofImg = null;
                await _unitOfWork.Ransoms.Add(ransom);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}

