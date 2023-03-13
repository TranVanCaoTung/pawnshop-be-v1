using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Requests
{
    public class CreateContractRequest
    {
        public string CustomerName { get; set; }
        public string IdentityCard { get; set; }
        public string ContractCode { get; set; }
        public int PawnableProductID { get; set; }
        public string ContractAssetName {get; set; }
        public decimal? InsuranceFee { get; set; }
        public decimal? StorageFee { get; set; }
        public decimal? loan { get; set; }
        public int UserId { get; set; }
        public ICollection<Models.Attribute>? PawnableAttributes { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int PackageId { get; set; }
        public int InterestRecommend { get; set; }   
    }
}
