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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class InterestDiaryService : IInteresDiaryService
    {
        private readonly IUnitOfWork _unit;
        private readonly InterestDiary _diary;
        private DbContextClass _dbContextClass;
        private readonly IInterestDiaryRepository _interestDiaryRepository;
        public InterestDiaryService(IUnitOfWork unitOfWork, DbContextClass dbContextClass, IInterestDiaryRepository interestDiaryRepository )
        {
            _unit = unitOfWork;
            _dbContextClass = dbContextClass;
            _interestDiaryRepository = interestDiaryRepository;

        }
        public async Task<bool> CreateInterestDiary(Contract contract)
        {
            if (contract != null)
            {
                int numberOfPeriods = contract.Package.Day/ contract.Package.PaymentPeriod;
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

        public async Task<InterestDiary> GetInteresDiaryByContractId(int contractId)
        {
            if (contractId != null)
            {
                return (InterestDiary) await _interestDiaryRepository.GetDiaryByContractId(contractId);              
            }
            return null;
        }

        public async Task<bool> UpdateInteresDiary(InterestDiary interestDiary)
        {
            var diaryUpdate = _unit.InterestDiaries.SingleOrDefault
                (interestDiary, j => j.InterestDiaryId == interestDiary.InterestDiaryId);
            if (diaryUpdate != null)
            {
                //diaryUpdate.TotalPay = diaryUpdate.Penalty + diaryUpdate.Payment;
                diaryUpdate.PaidMoney = interestDiary.PaidMoney;
                diaryUpdate.InterestDebt = diaryUpdate.TotalPay - interestDiary.PaidMoney;
                diaryUpdate.PaidDate = DateTime.Now;

                if (diaryUpdate.TotalPay == interestDiary.PaidMoney)
                {
                    diaryUpdate.Status = (int) InterestDiaryConsts.PAID;
                }
                diaryUpdate.Description = interestDiary.Description;
                diaryUpdate.ProofImg = interestDiary.ProofImg;
                _unit.InterestDiaries.Update(diaryUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
