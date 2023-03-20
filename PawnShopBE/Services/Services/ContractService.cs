using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
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
        public IContractAssetService _iContractAssetService;
        public IPackageService _iPackageService;
        public IInteresDiaryService _iInterestDiaryService;
        public IContractRepository _iContractRepository;
        private DbContextClass _dbContextClass;
        private IServiceProvider _serviceProvider;
        public ContractService(IUnitOfWork unitOfWork, IContractRepository iContractRepository,
            IContractAssetService contractAssetService, IPackageService packageService,
            IInteresDiaryService interesDiaryService, IServiceProvider serviceProvider, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
            _iContractRepository = iContractRepository;
            _iContractAssetService = contractAssetService;
            _iPackageService = packageService;
            _iInterestDiaryService = interesDiaryService;
            _serviceProvider = serviceProvider;
            _dbContextClass = dbContextClass;
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
                var package = await _iPackageService.GetPackageById(contract.PackageId, contract.InterestRecommend);

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
                contract.ContractEndDate = contract.ContractStartDate.AddDays((double)package.Day - 1);
                contract.Status = (int)ContractConst.IN_PROGRESS;
                await _unitOfWork.Contracts.Add(contract);

                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    var createRansom = await ransomProvider.CreateRansom(contract);
                    var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;
                    var createInterestDiary = await interestProvider.CreateInterestDiary(contract);
                    return true;
                }
                if (await plusPoint(contract))
                    return true;
            }
            return false;
        }
        private async Task<bool> plusPoint(Contract contract)
        {
            var provider= _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
            var customerList = await provider.GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == contract.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 100;
            if (await provider.UpdateCustomer(customer))
                return true;
            else
                return false;
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
            var result = await _unitOfWork.Contracts.TakePage(num, contractList);
            return result;
        }

        public async Task<ICollection<DisplayContractList>> GetAllDisplayContracts(int num)
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            //var contractJoinCustomerAndAsset = _dbContextClass.Contract
            //    .Include(c => c.Customer)
            //    .Include(c => c.ContractAsset)
            //    .ToListAsync();

            var contractJoinCustomerJoinAsset = from contract in _dbContextClass.Contract
                         join customer in _dbContextClass.Customer
                         on contract.CustomerId equals customer.CustomerId
                         join contractAsset in _dbContextClass.ContractAsset
                         on contract.ContractAssetId equals contractAsset.ContractAssetId
                         join pawnableProduct in _dbContextClass.PawnableProduct
                         on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                         join warehouse in _dbContextClass.Warehouse
                         on contractAsset.WarehouseId equals warehouse.WarehouseId
                         select new
                         {
                             ContractCode = contract.ContractCode,
                             CustomerName = customer.FullName,
                             CommodityCode = pawnableProduct.CommodityCode,
                             ContractAssetName = contractAsset.ContractAssetName,
                             ContractLoan = contract.Loan,
                             ContractStartDate = contract.ContractStartDate,
                             ContractEndDate = contract.ContractEndDate,
                             WarehouseName = warehouse.WarehouseName,
                             Status = contract.Status
                         };

            List<DisplayContractList> displayContractList = new List<DisplayContractList>();
            foreach (var row in contractJoinCustomerJoinAsset)
            {
                DisplayContractList displayContract = new DisplayContractList();
                displayContract.ContractCode = row.ContractCode;
                displayContract.CustomerName = row.CustomerName;
                displayContract.CommodityCode = row.CommodityCode;
                displayContract.ContractAssetName = row.ContractAssetName;
                displayContract.Loan = row.ContractLoan;
                displayContract.ContractStartDate = row.ContractStartDate;
                displayContract.ContractEndDate = row.ContractEndDate;
                displayContract.WarehouseName = row.WarehouseName;
                displayContract.Status = row.Status;
                displayContractList.Add(displayContract);
            }
            List<DisplayContractList> result = await _iContractRepository.displayContractListTakePage(num, displayContractList);        
            if (num == 0)
            {
                return displayContractList;
            }
            return result;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (contractList != null)
            {
                return contractList;

            }
            return null;
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

        public async Task<bool> UpdateContract(string contractCode, Contract contract)
        {
            if (contract != null)
            {
                var contractUpdate = await _iContractRepository.getContractByContractCode(contractCode);
                if (contractUpdate.ContractId == contract.ContractId)
                {
                    // Update Asset
                    if (contract.ContractAsset != null)
                    {
                        var assetUpdate = await _iContractAssetService.UpdateContractAsset(contract.ContractAsset);
                        if (assetUpdate == false)
                        {
                            return false;
                        }
                    }

                    contractUpdate.StorageFee = contract.StorageFee;
                    contractUpdate.InsuranceFee = contract.InsuranceFee;
                    contractUpdate.Loan = contract.Loan;
                    contractUpdate.ContractVerifyImg = contract.ContractVerifyImg;
                    contractUpdate.ActualEndDate = contract.ActualEndDate;
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
        public async Task<DisplayContractDetail> GetContractDetail(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                var customer = await _unitOfWork.Customers.GetById(contract.CustomerId);
                var package = await _iPackageService.GetPackageById(contract.PackageId, contract.InterestRecommend);
                List<InterestDiary> interestDiaries = (List<InterestDiary>)await _iInterestDiaryService.GetInteresDiariesByContractId(contractId);

                decimal interestPaid = 0;
                decimal interestDebt = 0;
                foreach (InterestDiary interestDiary in interestDiaries)
                {
                    interestPaid = interestPaid + interestDiary.PaidMoney;
                    interestDebt = interestDebt + interestDiary.InterestDebt;
                }
                var contractDetail = new DisplayContractDetail();
                contractDetail.CustomerName = customer.FullName;
                contractDetail.Phone = contract.Customer.Phone;
                contractDetail.Loan = contract.Loan;
                contractDetail.ContractStartDate = contract.ContractStartDate;
                contractDetail.ContractEndDate = contract.ContractEndDate;
                contractDetail.PackageInterest = package.PackageInterest;
                contractDetail.InterestPaid = interestPaid;
                contractDetail.InterestDebt = interestDebt;
                contractDetail.Status = contract.Status;

                if (contract != null)
                {
                    return contractDetail;
                }
            }
            return null;
        }

        public async Task<bool> UploadContractImg(int contractId, string customerImg, string contractImg)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);
            if (contract != null && (customerImg != null || contractImg != null))
            {
                contract.CustomerVerifyImg = customerImg;
                contract.ContractVerifyImg = contractImg;
            }
            _unitOfWork.Contracts.Update(contract);
            var result = _unitOfWork.Save();

            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> CreateContractExpiration(int contractId)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);
            //using var transaction = await _dbContextClass.Database.BeginTransactionAsync();
            //try
            //{
                if (contract != null)
                {
                    contract.ContractId = 0;
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
                    contract.ContractEndDate = contract.ContractStartDate.AddDays((double)package.Day - 1);
                    contract.Status = (int)ContractConst.IN_PROGRESS;
                    await _unitOfWork.Contracts.Add(contract);
                    var createContractresult = _unitOfWork.Save();
                    if (createContractresult > 0)
                    {
                        var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                        var createRansom = await ransomProvider.CreateRansom(contract);
                        var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;
                        var createInterestDiary = await interestProvider.CreateInterestDiary(contract);                      
                        return true;
                    }
                    //// If everything succeeded, commit the transaction
                    //await transaction.CommitAsync();
                }
            //} catch (Exception e)
            //{
            //    // If an error occurred, rollback the transaction
            //    await transaction.RollbackAsync();
            //    throw;
            //}
            
            return false;
        }

        
    }
}

