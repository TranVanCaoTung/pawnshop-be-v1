using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IInteresDiaryService
    {
        Task<bool> CreateInterestDiary(Contract contract);
        Task<IEnumerable<InterestDiary>> GetInteresDiary();
        Task<IEnumerable<InterestDiary>> GetInteresDiariesByContractId(int contractId);
        Task<bool> UpdateInterestDiary(int id, decimal paidMoney, List<string> proofImg);
        Task<bool> DeleteInteresDiary(int interestDiaryId);
        public Task<bool> UploadInterestDiaryImg(int interestDiaryId, string interestDiaryImg);
    }
}
