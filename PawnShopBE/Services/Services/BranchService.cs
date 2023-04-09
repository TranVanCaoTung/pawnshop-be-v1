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
        private ILedgerService _ledgerService;

        public BranchService(IUnitOfWork unitOfWork, IContractService contract
           , ILedgerService ledger, IInteresDiaryService diary)
        {
            _unitOfWork = unitOfWork;
            _contract = contract;
            _ledgerService = ledger;
            _diary = diary;
        }
        public async Task<DisplayBranchDetail> getDisplayBranchDetail(DisplayBranchDetail branchDetail)
        {
            //get all list
            var _ledgerList = await _ledgerService.GetLedger();
            var _contractList = await _contract.GetAllContracts(0);
            var _diaryList = await _diary.GetInteresDiary();
            //get branch id
            var branchID = branchDetail.branchId;
            //get ledger have branchID
            var ledger = from l in _ledgerList where l.BranchId == branchID select l;
            var contract = from c in _contractList where c.BranchId == branchID select c;
            //get contract id
            var contractId = getContractId(contract);
            //get diary have contractId
            var diary = from d in _diaryList where d.ContractId == contractId select d;

            foreach (var x in ledger)
            {
                //lãi đã thu
                branchDetail.recveivedInterest += x.RecveivedInterest;
                //tiền cho vay
                branchDetail.loanLedger += x.Loan;
                //quỹ tiền mặt   
                branchDetail.balance += x.Balance;
            }

            foreach (var x in contract)
            {
                //tiền đang cho vay
                branchDetail.loanContract += x.Loan;
                //lãi dự kiến
                branchDetail.totalProfit += x.TotalProfit;
            }
            //số hợp đồng
            branchDetail.numberContract = _contractList.Count();
            //số họp đồng mở
            branchDetail.openContract = getNumOpenCloseContract(1, _contractList);
            //số hợp đồng đóng
            branchDetail.closeContract = getNumOpenCloseContract(4, _contractList);

            foreach (var x in diary)
            {
                //tiền khách nợ
                branchDetail.debtCustomers += x.TotalPay - x.PaidMoney;
            }
            return branchDetail;
        }

        private int getNumOpenCloseContract(int status, IEnumerable<Contract> contractList)
        {
            switch (status)
            {
                case 1:
                    var listOpen = from c in contractList where c.Status == status select c;
                    var open = listOpen.Count();
                    return open;
                case 4:
                    var listClose = from c in contractList where c.Status == status select c;
                    var close = listClose.Count();
                    return close;
            }
            return 0;
        }
        private int getContractId(IEnumerable<Contract> contract)
        {
            var contractId = (from c in contract select c.ContractId).FirstOrDefault();
            return contractId;
        }
        public async Task<IEnumerable<DisplayBranch>> getDisplayBranch(IEnumerable<DisplayBranch> branchList)
        {
            //get list Ledger
            var _ledgerList = await _ledgerService.GetLedger();
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
                    branchUpdate.PhoneNumber = branch.PhoneNumber;
                    branchUpdate.Address = branch.Address;
                    branchUpdate.Fund = branch.Fund;
                    branchUpdate.BranchName = branch.BranchName;
                    branchUpdate.UpdateDate = DateTime.Now;
                    branchUpdate.Status = branch.Status;
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


       