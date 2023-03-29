using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(DbContextClass dbContext) : base(dbContext)
        {
        }

        public async Task<List<DisplayContractList>> displayContractListTakePage(int number, List<DisplayContractList> list)
        {
            var numPage = (int)NumberPage.numPage;
            var skip = (numPage * number) - numPage;
            return list.Skip(skip).Take(numPage).ToList();          
        }

        public async Task<Contract> getContractByContractCode(string contractCode)
        {
            Contract contract = _dbContext.Contract.SingleOrDefault(c => c.ContractCode == contractCode);
            return contract;
        }
    }
}
