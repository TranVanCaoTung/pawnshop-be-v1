using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Responses
{
    public class GetAllContractsResponse
    {
        public string ContractCode { get; }
        public string CustomerName { get; }
        public string CommodityCode { get; }
        public string AssetName { get; }
        public DateTime ContractStartDate { get; }
        public decimal? Loan { get; }

    }
}
