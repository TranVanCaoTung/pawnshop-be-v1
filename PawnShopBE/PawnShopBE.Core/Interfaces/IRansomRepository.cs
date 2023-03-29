using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface IRansomRepository : IGenericRepository<Ransom>
    {
        public Task<Ransom> GetRanSomByContractId(int contractId);
    }
}
