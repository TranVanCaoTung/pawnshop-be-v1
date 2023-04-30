using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IDiaryImgService
    {
        Task<IEnumerable<DiaryImg>> GetDiariesImg(int interestDiaryId);
        Task<bool> CreateDiariesImg(int interestDiaryId, List<string> prooImgs);
        Task<bool> UpdateDiariesImg(int interestDiaryId);
    }
}
