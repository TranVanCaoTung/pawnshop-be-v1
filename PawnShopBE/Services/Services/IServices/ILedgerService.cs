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
        Task<Ledger> GetLedgerById(int ledgerId);
        Task<bool> UpdateLedger(Ledger ledger);
        Task<bool> DeleteLedger(int ledgerId);
    }
}
