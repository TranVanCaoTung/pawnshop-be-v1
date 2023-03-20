using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BranchService : IBranchService
    {

        public IUnitOfWork _unitOfWork;
        private IContractService _contract;
        private IInteresDiaryService _diary;
        private ILedgerService _ledger;

        public BranchService(IUnitOfWork unitOfWork, IContractService contract
           , ILedgerService ledger, IInteresDiaryService diary)
        {
            _unitOfWork = unitOfWork;
            _contract = contract;
            _ledger = ledger;
            _diary = diary;
        }
        public async Task<DisplayBranchDetail> getDisplayBranchDetail(DisplayBranchDetail branchDetail)
        {
            //get list Ledger
            var _ledgerList = await _ledger.GetLedger();
            //get list contract
            var _contractList = await _contract.GetAllContracts(0);
            //get list Diary
            var _diaryList = await _diary.GetInteresDiary();
            //get branch id
            var branchID = branchDetail.branchId;

            //get ledger have branchID
            var ledger = from l in _ledgerList where l.BranchId == branchID select l;
            //get contract have branchID
            var contract = from c in _contractList where c.BranchId == branchID select c;

            //get contract id
            var contractId = getContractId(contract);

            //get diary have contractId
            var diary = from d in _diaryList where d.ContractId == contractId select d;

            //get Branch Detail
            foreach (var x in ledger)
            {
                branchDetail.recveivedInterest = x.RecveivedInterest;
                branchDetail.loanBranch = x.Loan;
                branchDetail.balance = x.Balance;
            }
            foreach (var x in contract)
            {
                branchDetail.loanContract = x.Loan;
                branchDetail.expectProfit= x.TotalProfit;
                branchDetail.contractCode = x.ContractCode;
                branchDetail.statusContract = x.Status;
            }
            // nợ khách cần trả
            foreach (var x in diary)
            {
                branchDetail.debtCustomers = x.TotalPay - x.PaidMoney;
            }
            return branchDetail;
        }
        private int getContractId(IEnumerable<Contract> contract)
        {
            var getContractId = from c in contract select c.ContractId;
            var contractId = getContractId.First();
            return contractId;
        }
        public async Task<IEnumerable<DisplayBranch>> getDisplayBranch(IEnumerable<DisplayBranch> branchList)
        {
            //get list Ledger
            var _ledgerList = await _ledger.GetLedger();
            foreach (var branch in branchList)
            {
                var branchId = branch.branchId;
                // ger ledger khi branch id = nhau
                var ledger = from l in _ledgerList where l.BranchId == branchId select l;
                foreach (var l in ledger)
                {
                    branch.recveivedInterest = l.RecveivedInterest;
                    branch.loan = l.Loan;
                    branch.balance = l.Balance;
                }
            }
            return branchList;
        }
        public async Task<bool> CreateBranch(Branch branch)
        {
            if (branch != null)
            {
                branch.CreateDate = DateTime.Now;
                await _unitOfWork.Branches.Add(branch);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteBranch(int branchId)
        {
            if (branchId != null)
            {
                var branch = await _unitOfWork.Branches.GetById(branchId);
                if (branch != null)
                {
                    _unitOfWork.Branches.Delete(branch);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Branch>> GetAllBranch(int num)
        {
            var branchList = await _unitOfWork.Branches.GetAll();
            if (num == 0)
            {
                return branchList;
            }
            var result= await _unitOfWork.Branches.TakePage(num,branchList);
            return result;
        }

        public async Task<Branch> GetBranchById(int branchId)
        {
            if (branchId != null)
            {
                var branch = await _unitOfWork.Branches.GetById(branchId);
                if (branch != null)
                {
                    return branch;
                }
            }
            return null;
        }

        public async Task<bool> UpdateBranch(int id, BranchRequest branch)
        {
            if (branch != null)
            {
                var branchUpdate = await _unitOfWork.Branches.GetById(id);
                if (branchUpdate != null)
                {
                    branchUpdate.BranchName = branch.BranchName;
                    branchUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Branches.Update(branchUpdate);

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


       