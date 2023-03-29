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
        public InterestDiaryService(IUnitOfWork unitOfWork, DbContextClass dbContextClass, IInterestDiaryRepository interestDiaryRepository, ILogContractService logContractService)
        {
            _unit = unitOfWork;
            _dbContextClass = dbContextClass;
            _interestDiaryRepository = interestDiaryRepository;
            _logContractService = logContractService;
        }

        private string GetCustomerName(Guid customerId)
        {
            var customerIenumerable = from c in _dbContextClass.Customer
                                      where c.CustomerId == customerId
                                      select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer.FullName;
        }

        private string GetUser(Guid userId)
        {
            var userIenumerable = from u in _dbContextClass.User
                                  where u.UserId == userId
                                  select u;
            var user = userIenumerable.FirstOrDefault();
            return user.FullName;
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
                    decimal payment = interestDiary.Payment = contract.TotalProfit / numberOfPeriods;
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

        public async Task<bool> UpdateInterestDiary(int id, decimal paidMoney)
        {
            try
            {
                var diaryUpdate = await _unit.InterestDiaries.GetById(id);
                if (diaryUpdate == null) return false;
                diaryUpdate.PaidMoney = paidMoney;
                diaryUpdate.InterestDebt = diaryUpdate.TotalPay - (paidMoney);
                diaryUpdate.PaidDate = DateTime.Now;

                if (diaryUpdate.TotalPay == paidMoney && diaryUpdate.InterestDebt == 0)
                {
                    diaryUpdate.Status = (int)InterestDiaryConsts.PAID;
                    diaryUpdate.InterestDebt = 0;
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
                    logContract.Description = diaryUpdate.NextDueDate.ToString("MM/dd/yyyy HH:mm");
                    logContract.EventType = (diaryUpdate.TotalPay == diaryUpdate.PaidMoney) ? (int)LogContractConst.INTEREST_PAID : (int)LogContractConst.INTEREST_NOT_PAID;
                    logContract.LogTime = DateTime.Now;
                    if (logContract.EventType == (int)LogContractConst.INTEREST_PAID)
                    {
                        await _logContractService.CreateLogContract(logContract);
                    }
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
                    interestDiary.ProofImg = interestDiaryImg;
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
