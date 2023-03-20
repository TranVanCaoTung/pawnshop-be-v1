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
        private IContractService _contract;
        private IPackageService _package;
        private ICustomerService _customer;

        public RansomService(IUnitOfWork unitOfWork, IContractService contract, IPackageService package, ICustomerService customer)
        {
            _unitOfWork = unitOfWork;
            _contract = contract;
            _package = package;
            _customer = customer;
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
        private async Task<bool> getAll_field_plus_point(Ransom ransom)
        {
            //get List package
            var packageList = await _package.GetAllPackages(0);

            //get List customer
            var customerList = await _customer.GetAllCustomer(0);

            //get List contract
            var contractList=await _contract.GetAllContracts(0);

            //get contract
            var contractIenumerable=from c in contractList where c.ContractId== ransom.ContractId select c;
            var contract= contractIenumerable.FirstOrDefault();

            //get customer
            var customerIenumerable= from c in customerList where c.CustomerId==contract.CustomerId select c;
            var customer= customerIenumerable.FirstOrDefault();

            //get package
            var packageIenumerable = from p in packageList where p.PackageId == contract.PackageId select p;
            var package= packageIenumerable.FirstOrDefault();

            if (await plusPoint(customer, package, ransom))
            {
                return true;
            }
            return false;
        }

        private async Task<bool> plusPoint(Customer customer,Package package,Ransom ransom)
        {
            //thanh toán đúng hạn contract 3 tháng trở lên
            if (package.Day >= 90)
            {
                customer.Point += 50;
                await _customer.UpdateCustomer(customer);
                return true;
            }
            //thanh toán đúng hạn
            if(ransom.Status==(int)RansomConsts.ON_TIME)
            {
                customer.Point += 20;
                await _customer.UpdateCustomer(customer); 
                return true;
            }
            //thanh toán đúng hạn 5 hợp đồng 3 tháng trở lên

            return false;
        }

    }
}

