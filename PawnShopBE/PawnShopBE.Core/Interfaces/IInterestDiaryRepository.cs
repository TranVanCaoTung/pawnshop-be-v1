using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PawnShopBE.Core.Interfaces
{
    public interface IInterestDiaryRepository : IGenericRepository<InterestDiary>
    {
        public Task<IEnumerable<InterestDiary>> GetDiaryByContractId(int contractId);
        
    }
}
