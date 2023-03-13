using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBranchRepository Branches { get; }
        IRoleRepository Roles { get; }
        ILedgerRepository Ledgers { get; }
        IInterestDiaryRepository InterestDiaries { get; }
        IContractRepository Contracts { get; }
        IKycRepository Kycs { get; }
        IPackageRepository Packages { get; }
        ILiquidationRepository Liquidations { get; }
        ICustomerRepository Customers { get; }
        IContractAssetRepository ContractAssets { get; }
        IPawnableProductRepository PawnableProduct { get; }
        IAttributeRepository Attributes { get; }
        IWarehouseRepository Warehouses { get; }
        IDependentPeopleRepository DependentPeople { get; }
        IJobRepository Jobs { get; }
        ICustomerRelativeRelationshipRepository CustomersRelativeRelationships { get; }
        IRansomRepository Ransoms { get; }
        int Save();
        Task<int> SaveList();
    }
}
