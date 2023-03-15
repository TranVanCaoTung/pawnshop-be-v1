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
                ransom.Payment = contract.Loan;
                ransom.PaidMoney = 0;
                ransom.PaidDate = null;
                ransom.Status = (int) RansomConsts.SOON;
                ransom.Description = null;
                ransom.ProofImg = null;
                if (contract.Package.Day < 120)
                {
                    ransom.Penalty = 0;
                }
                // Penalty for pay all before duedate (50% interest paid & contract must > 6 months)
                // Penalty for contract 6 months
                else if (contract.Package.Day == 120)
                {
                    ransom.Penalty = ransom.Payment *(decimal) 0.03;
                }
                // Penalty for contract 9 months
                else if (contract.Package.Day == 270)
                {
                    ransom.Penalty = ransom.Payment * (decimal) 0.04;
                }
                // Penalty for contract 12 months
                else if (contract.Package.Day == 360)
                {
                    ransom.Penalty = ransom.Payment * (decimal) 0.05;
                }
                ransom.TotalPay = contract.Loan + contract.TotalProfit + ransom.Penalty;
                
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

