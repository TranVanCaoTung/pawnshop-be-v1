using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MoneyService : IMoneyService
    {
        private readonly IUnitOfWork _unit;
        public MoneyService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<bool> CreateMoney(Money money)
        {
            if (money != null)
            {
                money.DateCreate = DateTime.Now;
                await _unit.Money.Add(money);
                // cộng dồn thêm tiền vốn
               var branch = await getBranch(money.BranchId);
                branch.Fund += money.MoneyInput;
                _unit.Branches.Update(branch);
                var result = _unit.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        private async Task<Branch> getBranch(int branchId)
        {
           var list = await _unit.Branches.GetAll();
           var branch = (from b in list where b.BranchId == branchId select b).FirstOrDefault();
           return branch;
        }

        public async Task<IEnumerable<Money>> GetAllMoney(int num, int branchId)
        {
            var list = await _unit.Money.GetAll();
            //select money theo branch 
            var moneyList = from b in list where b.BranchId==branchId select b;
            //get branch
            var branch = await getBranch(branchId);
            if (num == 0)
            {
                return moneyList;
            }
            var result = await _unit.Money.TakePage(num, moneyList);
            return result;
        }
    }
}
