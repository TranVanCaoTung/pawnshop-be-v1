using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class ContractDTO
    {
        public int ContractId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public int BranchId { get; set; }
        public int WarehouseId { get; set; }
        public int PawnableProductId { get; set; }
        public int ContractAssetId { get; }
        public int PackageId { get; set; }
       
        public string ContractAssetName { get; set; }
        
        public decimal InsuranceFee { get; set; }
        public decimal StorageFee { get; set; }
        
        public decimal Loan { get; set; }
        public decimal TotalProfit { get; set; }
        public string AssetImg { get; set; }
        public ICollection<AttributeDTO>? PawnableAttributeDTOs { get; set; }
        public int InterestRecommend { get; set; }
        public string? Description { get; set; }

        public ContractDTO()
        {
            InsuranceFee = 0;
            StorageFee = 0;
            InterestRecommend = 0;
        }
    }
}
