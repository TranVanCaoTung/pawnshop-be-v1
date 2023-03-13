using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ContractService : IContractService
    {
        public IUnitOfWork _unitOfWork;
        public ContractAssetService _contractAssetService;

        public ContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Contract> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                contract.Branch = null;
                contract.Package = null;
                contract.Customer = null;
                contract.ContractAsset = null;
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }                               
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
                var package = await _unitOfWork.Packages.GetById(contract.PackageId);
                
                if (package != null)
                {
                    
                    var fee = contract.InsuranceFee + contract.StorageFee;
                    var period = package.Day / package.PaymentPeriod;
                    double interest = 0;
                    
                    // Use recommend interest if input
                    if (contract.InterestRecommend != 0)
                    {
                        interest = contract.InterestRecommend * 0.01;
                    }
                    interest = package.PackageInterest * 0.01;
                    contract.TotalProfit = (contract.Loan * (decimal)interest) + (fee * period);                   
                }
                contract.ContractStartDate = DateTime.Now;
                contract.ContractEndDate = contract.ContractStartDate.AddDays((double)package.Day -1);
                contract.Status = (int)ContractConst.IN_PROGRESS;
                await _unitOfWork.Contracts.Add(contract);

                var result = _unitOfWork.Save();

                //if (result > 0)
                //    return true;
                //else
                //    return false;
            }
            return contract;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    _unitOfWork.Contracts.Delete(contract);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts(int num)
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (num == 0)
            {
                return contractList;
            }
            var result= await _unitOfWork.Contracts.TakePage(num,contractList);
            return result;
        }

        public async Task<Contract> GetContractById(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    return contract;
                }
            }
            return null;
        }

        public async Task<bool> UpdateContract(Contract contract)
        {
            if (contract != null)
            {
                var contractUpdate = await _unitOfWork.Contracts.GetById(contract.ContractId);

                if (contractUpdate != null)
                {
                    // Update Asset
                    if (contract.ContractAsset != null)
                    {
                        var assetUpdate = await _contractAssetService.UpdateContractAsset(contract.ContractAsset);
                        if (assetUpdate == false)
                        {
                            return false;
                        }
                    }

                    contractUpdate.StorageFee = contract.StorageFee;
                    contractUpdate.InsuranceFee = contract.InsuranceFee;
                    contractUpdate.Loan = contract.Loan;                          
                    contractUpdate.ContractVerifyImg = contract.ContractVerifyImg;
                    contractUpdate.UpdateDate = DateTime.Now;

                    var package = await _unitOfWork.Packages.GetById(contract.PackageId);

                    if (package != null)
                    {

                        var fee = contractUpdate.InsuranceFee + contractUpdate.StorageFee;
                        var period = package.Day / package.PaymentPeriod;
                        double interest = 0;

                        // Use recommend interest if input
                        if (contractUpdate.InterestRecommend != 0)
                        {
                            interest = contractUpdate.InterestRecommend * 0.01;
                        }
                        interest = package.PackageInterest * 0.01;
                        contractUpdate.TotalProfit = (contractUpdate.Loan * (decimal)interest + fee) * period;
                    }

                    _unitOfWork.Contracts.Update(contractUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
