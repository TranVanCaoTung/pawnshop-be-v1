using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IRansomService
    {
        Task<bool> CreateRansom(Contract contract);
        Task<IEnumerable<Ransom>> GetRansom();
        Task<Ransom> GetRansomByContractId(int contractId);
        Task<bool> SaveRansom(int ransomId);
    }
}
