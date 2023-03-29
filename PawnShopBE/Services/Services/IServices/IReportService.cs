using PawnShopBE.Core.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IReportService
    {
        Task<IEnumerable<DisplayReportTransaction>> getReportTransaction(int number);
        Task<IEnumerable<DisplayReportMonth>> getReportMonth(int branchId);
    }
}
