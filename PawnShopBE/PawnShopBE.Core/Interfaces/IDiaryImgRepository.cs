using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface IDiaryImgRepository : IGenericRepository<DiaryImg>
    {
        public Task<IEnumerable<DiaryImg>> GetDiaryImgByInterestDiaryId(int interestDiary);

    }
}
