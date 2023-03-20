using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        Task<Contract> getContractByContractCode(string contractCode);
        Task<List<DisplayContractList>> displayContractListTakePage(int number, List<DisplayContractList> list);
    }
}
