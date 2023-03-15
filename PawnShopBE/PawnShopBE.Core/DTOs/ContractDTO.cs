using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
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
        //[Required(ErrorMessage = "Tên khách hàng không được để trống")]
        //[StringLength(50, MinimumLength = 6, 
        //    ErrorMessage = "Tên khách hàng phải dài từ 6 - 50 ký tự")]
        //public string CustomerName { get; set; }
        //public string ContractCode { get; set; }

        [Required(ErrorMessage = "Tên tài sản không được để trống")]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Tên tài sản phải dài từ 6 - 50 ký tự")]
        public string ContractAssetName { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal InsuranceFee { get; set; }

        public decimal StorageFee { get; set; }

        [Range(1000, 100000000, ErrorMessage = "Tiền nhập phải từ 1000 - 100Tr")]
        public decimal Loan { get; set; }
        public decimal TotalProfit { get; set; }

        public string AssetImg { get; set; }
        public ICollection<AttributeDTO>? PawnableAttributeDTOs { get; set; }

        [Required(ErrorMessage = "Căn cước công dân không được để trống")]
        public string CCCD { get; set; }      
        //[Phone(ErrorMessage ="Nhập đúng định dạng số điện thoại")]
        //public string CustomerPhoneNumber { get; set; }
        //[Required(ErrorMessage = "Địa chỉ khách hàng không được để trống")]
        //public string CustomerAddress { get; set; }
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
