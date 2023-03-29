using AutoMapper;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace PawnShopBE.Helpers
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            // Mapping tu DTO sang entity

            #region Customer
            //Mapping from relarive_job to DisplayCus
            CreateMap<Customer, Relative_Job_DependentDTO>().ReverseMap();

            // Mapping from Customer to Display Customer
            CreateMap<Customer,DisplayCustomer>().ReverseMap();

            // Mapping from Customer to Contract
            CreateMap<Contract, Customer>().
                ForMember(customer => customer.CustomerId,
                contract => contract.MapFrom(src => src.CustomerId));        
            // Mapping from CustomerDTO to Customer
            CreateMap<CustomerDTO, Customer>()
                .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName))
                .ForMember(
                dest => dest.CCCD,
                opt => opt.MapFrom(src => src.CCCD))
                .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
                .ForMember(
                dest => dest.Phone,
                opt => opt.MapFrom(src => src.Phone))
                .ForMember(
                dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(
                dest => dest.Point,
                opt => opt.MapFrom(src => src.Point));
            #endregion Customer

            #region Contract
            // Mapping from Contract to Branch
            CreateMap<Branch, Contract>().ForMember(contract => contract.BranchId,
                branch => branch.MapFrom(src => src.BranchId));

            // Mapping from ContractDTO to ContractAsset
            CreateMap<ContractDTO, ContractAsset>()
                .ForMember(
                dest => dest.ContractAssetName,
                opt => opt.MapFrom(src => src.ContractAssetName))
                .ForMember(
                dest => dest.PawnableProductId,
                opt => opt.MapFrom(src => src.PawnableProductId))
                .ForMember(
                dest => dest.WarehouseId,
                opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(src => src.AssetImg));

            // Mapping from ContractAssetDTO to ContractAsset
            CreateMap<ContractAssetDTO, ContractAsset>()
                .ForMember(
                dest => dest.ContractAssetName,
                opt => opt.MapFrom(src => src.ContractAssetName))
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
                .ForMember(
                dest => dest.PawnableProductId,
                opt => opt.MapFrom(src => src.PawnableProductId));

            // Mapping from ContractDTO to Contract
            CreateMap<ContractDTO, Contract>()
                .ForMember(
                dest => dest.BranchId,
                opt => opt.MapFrom(src => src.BranchId))               
                .ForMember(
                dest => dest.CustomerId,
                opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(
                dest => dest.PackageId,
                opt => opt.MapFrom(src => src.PackageId))
                .ForMember(
                dest => dest.ContractAssetId,
                opt => opt.MapFrom(src => src.ContractAssetId))
                .ForMember(
                dest => dest.Loan,
                opt => opt.MapFrom(src => src.Loan))
                .ForMember(
                dest => dest.InsuranceFee,
                opt => opt.MapFrom(src => src.InsuranceFee))
                .ForMember(
                dest => dest.StorageFee,
                opt => opt.MapFrom(src => src.StorageFee))
                .ForMember(
                dest => dest.TotalProfit,
                opt => opt.MapFrom(src => src.TotalProfit))
                 .ForMember(
                dest => dest.InterestRecommend,
                opt => opt.MapFrom(src => src.InterestRecommend));      
            #endregion Contract

            #region Branch
            CreateMap<Branch, DisplayBranch>().ReverseMap();
            CreateMap<Branch, DisplayBranchDetail>().ReverseMap();
            CreateMap<BranchDTO, Branch>().ReverseMap();
            #endregion Branch

            CreateMap<PawnableDTO, PawnableProduct>()
                .ForMember(
                    dest => dest.Attributes,
                    opt => opt.MapFrom(src => src.AttributeDTOs));
            CreateMap<Ledger, LedgerDTO>().ReverseMap();
            CreateMap<AttributeDTO,Attribute>();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Contract, ContractDTO>().ReverseMap();         
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<AttributeDTO, Attribute>();
            CreateMap<PawnableDTO, PawnableProduct>().ReverseMap();
            CreateMap<Kyc,KycDTO>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();
            CreateMap<Warehouse, WareHouseDTO>().ReverseMap();
            CreateMap<LiquidationDTO,Liquidtation>().ReverseMap();
            CreateMap<InterestDiary, InterestDiaryDTO>().ReverseMap();
            CreateMap<ContractAsset, ContractAssetDTO>().ReverseMap();
            CreateMap<DependentPeople, DependentPeopleDTO>().ReverseMap();
            CreateMap<CustomerRelativeDTO, CustomerRelativeRelationship>().ReverseMap();
        }
    }
}
