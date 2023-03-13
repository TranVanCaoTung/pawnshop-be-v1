using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PawnShopBE.Core.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public Guid CustomerId { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
        public int ContractAssetId { get; set; }
        public string ContractCode { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int InterestRecommend { get; set; } = 0;
        public decimal Loan { get; set; }
        public decimal InsuranceFee { get; set; } = 0;
        public decimal StorageFee { get; set; } = 0;
        public decimal TotalProfit { get; set; }
        public string? ContractVerifyImg { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }


        public virtual Customer? Customer { get; set; }
        public virtual Package? Package { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual ContractAsset? ContractAsset { get; set; }
        public virtual Liquidtation? Liquidtation { get; set; }
        public virtual Ransom? Ransom { get; set; }

        public ICollection<InterestDiary>? InterestDiaries { get; set; }
        public Contract()
        {
            InterestDiaries = new List<InterestDiary>();
        }
    }
}
