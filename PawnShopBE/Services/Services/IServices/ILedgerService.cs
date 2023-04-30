using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface ILedgerService
    {
        Task<bool> CreateLedger(Ledger ledger);
        Task<IEnumerable<Ledger>> GetLedger();
        Task<IEnumerable<Ledger>> GetLedgersByBranchId(int branchId, int year);
        Task<IEnumerable<Ledger>> GetLedgersByBranchId(int branchId);
        Task<IEnumerable<int>> GetYearsOfLedger();
        Task<bool> UpdateLedger(Ledger ledger);
    }
}
