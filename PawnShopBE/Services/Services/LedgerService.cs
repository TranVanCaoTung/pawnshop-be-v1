using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LedgerService : ILedgerService
    {
        private readonly IUnitOfWork _unit;
        private readonly Ledger _ledger;

        public LedgerService(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        public async Task<bool> CreateLedger(Ledger ledger)
        { 
            if (ledger != null)
            {
                await _unit.Ledgers.Add(ledger);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteLedger(int ledgerId)
        {
            var ledgerDelete = _unit.Ledgers.SingleOrDefault(_ledger, j => j.LedgerId == ledgerId);;
            if (ledgerDelete != null)
            {
                _unit.Ledgers.Delete(ledgerDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Ledger>> GetLedger()
        {
            var result = await _unit.Ledgers.GetAll();
            return result;
        }

        public Task<Ledger> GetLedgerById(int ledgerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateLedger(Ledger ledger)
        {
            var ledgerUpdate = _unit.Ledgers.SingleOrDefault(ledger, j => j.LedgerId == ledger.LedgerId);
            if (ledgerUpdate != null)
            {
                ledgerUpdate.BranchId = ledger.BranchId;
                ledgerUpdate.ReceivedPrincipal = ledger.ReceivedPrincipal;
                ledgerUpdate.RecveivedInterest = ledger.RecveivedInterest;
                ledgerUpdate.Loan = ledger.Loan;
                ledgerUpdate.Balance = ledger.Balance;
                ledgerUpdate.FromDate = ledger.FromDate;
                ledgerUpdate.ToDate = ledger.FromDate;
                ledgerUpdate.Status = ledger.Status;
                _unit.Ledgers.Update(ledgerUpdate);
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
