using Microsoft.Extensions.DependencyInjection;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
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
        private IContractService _contract;
        private IPackageService _package;
        private ICustomerService _customer;
        private IRansomRepository _ransomRepository;
        private ILogContractService _logContractService;
        private DbContextClass _dbContextClass;

        public RansomService(IUnitOfWork unitOfWork, IContractService contract, IPackageService package, ICustomerService customer, IRansomRepository ransomRepository, ILogContractService logContractService, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
            _contract = contract;
            _package = package;
            _customer = customer;
            _ransomRepository = ransomRepository;
            _logContractService = logContractService;
            _dbContextClass = dbContextClass;
        }
        public async Task<IEnumerable<Ransom>> GetRansom()
        {
            var result = await _unitOfWork.Ransoms.GetAll();
            return result;
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
                ransom.Status = (int)RansomConsts.SOON;
                ransom.Description = null;
                ransom.ProofImg = null;
                if (contract.Package.Day < 120)
                {
                    ransom.Penalty = 0;
                }
                // Penalty for pay all before duedate (50% interest paid & contract must > 6 months)
                // Penalty for contract 6 months
                else if (contract.Package.Day == 180)
                {
                    ransom.Penalty = ransom.Payment * (decimal)0.03;
                }
                // Penalty for contract 9 months
                else if (contract.Package.Day == 270)
                {
                    ransom.Penalty = ransom.Payment * (decimal)0.04;
                }
                // Penalty for contract 12 months
                else if (contract.Package.Day == 360)
                {
                    ransom.Penalty = ransom.Payment * (decimal)0.05;
                }
                ransom.TotalPay = contract.Loan + ransom.Penalty;

                await _unitOfWork.Ransoms.Add(ransom);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> SaveRansom(int ransomId, string proofImg)
        {
            var ransomList = await GetRansom();
            var ransom = (from r in ransomList where r.RansomId == ransomId select r).FirstOrDefault();
            if (ransom != null)
            {
                var contractList = await _contract.GetAllContracts(0);
                var contract = (from c in contractList where c.ContractId == ransom.ContractId select c).FirstOrDefault();
                contract.ActualEndDate = DateTime.Now;
                contract.Status = (int)ContractConst.CLOSE;
                await _contract.UpdateContract(contract.ContractId, contract);
                var result = await getAll_field_plus_point(ransom, ransom.Status);
                ransom.ProofImg = proofImg;
                ransom.PaidDate = DateTime.Now;
                ransom.PaidMoney = ransom.TotalPay;
                _unitOfWork.Ransoms.Update(ransom);
                if (result)
                {
                    // Close Log Contract
                    var contractJoinUserJoinCustomer = from getcontract in _dbContextClass.Contract
                                                       join customer in _dbContextClass.Customer
                                                       on contract.CustomerId equals customer.CustomerId
                                                       join user in _dbContextClass.User
                                                       on contract.UserId equals user.UserId
                                                       select new
                                                       {
                                                           ContractId = contract.ContractId,
                                                           UserName = user.FullName,
                                                           CustomerName = customer.FullName,
                                                       };
                    var logContract = new LogContract();
                    foreach (var row in contractJoinUserJoinCustomer)
                    {
                        logContract.ContractId = row.ContractId;
                        logContract.UserName = row.UserName;
                        logContract.CustomerName = row.CustomerName;
                    }
                    logContract.Debt = contract.Loan;
                    logContract.Paid = contract.Loan;
                    logContract.LogTime = DateTime.Now;
                    logContract.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    logContract.EventType = (int)LogContractConst.CLOSE_CONTRACT;
                    await _logContractService.CreateLogContract(logContract);
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> getAll_field_plus_point(Ransom ransom, int status)
        {
            //get List all
            var packageList = await _package.GetAllPackages(0);
            var customerList = await _customer.GetAllCustomer(0);
            var contractList = await _contract.GetAllContracts(0);

            //get contract
            var contract = (from c in contractList where c.ContractId == ransom.ContractId select c).FirstOrDefault();
            //get customer
            var customer = (from c in customerList where c.CustomerId == contract.CustomerId select c).FirstOrDefault();
            //get package
            var package = (from p in packageList where p.PackageId == contract.PackageId select p).FirstOrDefault();

            if (await plusPoint(customer, package, ransom, status))
            {
                return true;
            }
            return false;
        }

        private async Task<bool> plusPoint(Customer customer, Package package, Ransom ransom, int status)
        {
            switch (status)
            {
                case 1:
                    //thanh toán sai hạn
                    if (ransom.Status == (int)RansomConsts.SOON)
                    {
                        customer.Point -= 20;
                        await _customer.UpdateCustomer(customer);
                        return true;
                    }
                    break;
                case 2:
                    //thanh toán đúng hạn contract 3 tháng trở lên
                    if (package.Day >= 90)
                    {
                        customer.Point += 50;
                        await _customer.UpdateCustomer(customer);
                        return true;
                    }
                    //thanh toán đúng hạn
                    if (ransom.Status == (int)RansomConsts.ON_TIME)
                    {
                        customer.Point += 20;
                        await _customer.UpdateCustomer(customer);
                        return true;
                    }
                    break;
                case 3:
                    if (ransom.Status == (int)RansomConsts.LATE)
                    {
                        customer.Point -= 20;
                        await _customer.UpdateCustomer(customer);
                        return true;
                    }
                    break;
            }

            return false;
        }

        public Task<Ransom> GetRansomByContractId(int contractId)
        {
            var ransom = _ransomRepository.GetRanSomByContractId(contractId);
            return ransom;
        }
    }
}

