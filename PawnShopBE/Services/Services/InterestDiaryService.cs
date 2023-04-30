using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Helpers;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Contract = PawnShopBE.Core.Models.Contract;

namespace Services.Services
{
    public class InterestDiaryService : IInteresDiaryService
    {
        private readonly IUnitOfWork _unit;
        private readonly InterestDiary _diary;
        private DbContextClass _dbContextClass;
        private readonly IInterestDiaryRepository _interestDiaryRepository;
        private readonly ILogContractService _logContractService;
        private readonly IDiaryImgService _diaryImgService;
        public InterestDiaryService(IUnitOfWork unitOfWork, DbContextClass dbContextClass, IInterestDiaryRepository interestDiaryRepository, ILogContractService logContractService, IDiaryImgService diaryImgService)
        {
            _unit = unitOfWork;
            _dbContextClass = dbContextClass;
            _interestDiaryRepository = interestDiaryRepository;
            _logContractService = logContractService;
            _diaryImgService = diaryImgService;
        }

        public async Task<bool> CreateInterestDiary(Contract contract)
        {
            if (contract != null)
            {
                int numberOfPeriods = contract.Package.Day / contract.Package.PaymentPeriod;
                DateTime startDate = contract.ContractStartDate;
                DateTime endDate = contract.ContractEndDate;
                List<Tuple<DateTime, DateTime>> periods = HelperFuncs.DivideTimePeriodIntoPeriods(startDate, endDate, numberOfPeriods);
                var result = 0;

                // Payment for each period
                foreach (var period in periods)
                {
                    InterestDiary interestDiary = new InterestDiary();
                    ICollection<InterestDiary> interestDiaries = new List<InterestDiary>();

                    // Interest money
                    decimal payment = interestDiary.Payment = (int) contract.TotalProfit / numberOfPeriods;

                    decimal totalFee = contract.InsuranceFee + contract.StorageFee;

                    interestDiary.ContractId = contract.ContractId;
                    interestDiary.DueDate = period.Item1;
                    interestDiary.NextDueDate = period.Item2;

                    interestDiary.Status = (int)InterestDiaryConsts.NOT_PAID;
                    interestDiary.TotalPay = payment;
                    interestDiary.Penalty = 0;
                    interestDiary.PaidMoney = 0;
                    interestDiary.InterestDebt = 0;
                    _dbContextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary ON;");
                    interestDiaries.Add(interestDiary);
                    await _unit.InterestDiaries.AddList(interestDiaries);
                    _dbContextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary OFF;");

                }
                result = await _unit.SaveList();

                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteInteresDiary(int interestDiaryId)
        {

            var diaryDelete = _unit.InterestDiaries.SingleOrDefault(_diary, j => j.InterestDiaryId == interestDiaryId);
            if (diaryDelete != null)
            {
                _unit.InterestDiaries.Delete(diaryDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<InterestDiary>> GetInteresDiary()
        {
            var result = await _unit.InterestDiaries.GetAll();
            return result;
        }

        public async Task<IEnumerable<InterestDiary>> GetInteresDiariesByContractId(int contractId)
        {
            try
            {
                return (contractId != null) ? (List<InterestDiary>)await _interestDiaryRepository.GetDiaryByContractId(contractId) : null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return null;
        }

        public async Task<bool> UpdateInterestDiary(int id, decimal paidMoney, List<string> proofImg)
        {
            try
            {
                var diaryUpdate = await _unit.InterestDiaries.GetById(id);
                if (diaryUpdate == null) return false;          
                // Check current PaidMoney
                if (paidMoney > diaryUpdate.TotalPay || (paidMoney + diaryUpdate.PaidMoney) > diaryUpdate.TotalPay)
                {
                    return false;
                }
                if (diaryUpdate.PaidMoney == diaryUpdate.TotalPay)
                {
                    paidMoney = 0;
                }
                diaryUpdate.InterestDebt = diaryUpdate.TotalPay - (paidMoney + diaryUpdate.PaidMoney);
                if (diaryUpdate.InterestDebt < 0)
                {
                    diaryUpdate.InterestDebt = 0;
                }
                diaryUpdate.PaidMoney += paidMoney;
                // Status is PAID
                if (diaryUpdate.TotalPay == diaryUpdate.PaidMoney && diaryUpdate.InterestDebt == 0)
                {
                    diaryUpdate.Status = (int)InterestDiaryConsts.PAID;
                }

                if (diaryUpdate.InterestDebt != 0)
                {
                    diaryUpdate.Status = (int)InterestDiaryConsts.DEBT;

                }
                _unit.InterestDiaries.Update(diaryUpdate);

                var result = _unit.Save();
                if (result > 0)
                {
                    // Log Contract when onTime
                    var contractJoinUserJoinCustomer = from contract in _dbContextClass.Contract
                                                       join customer in _dbContextClass.Customer
                                                       on contract.CustomerId equals customer.CustomerId
                                                       join user in _dbContextClass.User
                                                       on contract.UserId equals user.UserId
                                                       where contract.ContractId == diaryUpdate.ContractId
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
                    logContract.Debt = diaryUpdate.TotalPay;
                    logContract.Paid = diaryUpdate.PaidMoney;
                    logContract.Description = diaryUpdate.NextDueDate.ToString("dd/MM/yyyy HH:mm");
                    logContract.EventType = (diaryUpdate.TotalPay == diaryUpdate.PaidMoney) ? (int)LogContractConst.INTEREST_PAID : (int)LogContractConst.INTEREST_NOT_PAID;
                    if (diaryUpdate.Status == (int)InterestDiaryConsts.DEBT || diaryUpdate.PaidMoney < paidMoney)
                    {
                        logContract.EventType = (int)LogContractConst.INTEREST_DEBT;
                        logContract.Debt = diaryUpdate.InterestDebt;
                        logContract.Paid = paidMoney;
                    }
                    logContract.LogTime = DateTime.Now;
                    await _logContractService.CreateLogContract(logContract);
                   
                        await _diaryImgService.CreateDiariesImg(id, proofImg);
                


                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return false;

        }

        public async Task<bool> UploadInterestDiaryImg(int interestDiaryId, string interestDiaryImg)
        {
            try
            {
                var interestDiary = await _unit.InterestDiaries.GetById(interestDiaryId);
                if (interestDiary != null && (interestDiaryImg != null))
                {
                    //interestDiary.ProofImg = interestDiaryImg;
                }
                _unit.InterestDiaries.Update(interestDiary);
                var result = _unit.Save();

                return (result > 0) ? true : false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return false;
        }


    }
}
