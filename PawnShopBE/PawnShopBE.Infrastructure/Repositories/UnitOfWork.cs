using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;
        public IUserRepository Users { get; }

        public IBranchRepository Branches { get; }

        public IRoleRepository Roles { get; }

        public ILedgerRepository Ledgers { get; }

        public IInterestDiaryRepository InterestDiaries { get; }

        public IContractRepository Contracts { get; }

        public IKycRepository Kycs { get; }

        public IPackageRepository Packages { get; }

        public ILiquidationRepository Liquidations { get; }

        public ICustomerRepository Customers { get; }

        public IContractAssetRepository ContractAssets { get; }

        public IPawnableProductRepository PawnableProduct { get; }

        public IAttributeRepository Attributes { get; }

        public IWarehouseRepository Warehouses { get; }

        public IDependentPeopleRepository DependentPeople { get; }

        public IJobRepository Jobs { get; }

        public ICustomerRelativeRelationshipRepository CustomersRelativeRelationships { get; }

        public IRansomRepository Ransoms { get; }
        public IPermissionReporsitory Permission { get; }
        public IUserPermissionGroupRepository UserPermissionGroup { get; }
        public IMoneyRepository Money { get; }
        public ILogContractRepository LogContracts { get; }
        public UnitOfWork(  DbContextClass dbContext,
                            IUserRepository userRepository, 
                            IBranchRepository branchRepository,
                            IRoleRepository roleRepository,
        ILedgerRepository ledgerRepository,
        IInterestDiaryRepository interestDiaryRepository,
        IContractRepository contractRepository,
        IKycRepository kycRepository,
        IPackageRepository packageRepository,
        ILiquidationRepository liquidationRepository,
        ICustomerRepository customerRepository,
        IContractAssetRepository contractAssetRepository,
        IPawnableProductRepository pawnableProductRepository,
        IAttributeRepository attributeRepository,
        IWarehouseRepository warehouseRepository,
        IDependentPeopleRepository dependentPeopleRepository,
        IJobRepository jobRepository,
        ICustomerRelativeRelationshipRepository customerRelativeRelationshipRepository,
        IRansomRepository ransomRepository,IPermissionReporsitory permissionReporsitory,
        IUserPermissionGroupRepository userPermissionGroupRepository,IMoneyRepository moneyRepository,
        ILogContractRepository logContractRepository)
        {
            _dbContext = dbContext;
            Users = userRepository;
            Branches = branchRepository;
            Roles = roleRepository;
            Ledgers = ledgerRepository;
            InterestDiaries = interestDiaryRepository;
            Contracts = contractRepository;
            Kycs = kycRepository;
            Packages = packageRepository;
            Liquidations = liquidationRepository;
            Customers = customerRepository;
            ContractAssets = contractAssetRepository;
            PawnableProduct = pawnableProductRepository;
            Attributes = attributeRepository;
            Warehouses = warehouseRepository;
            DependentPeople = dependentPeopleRepository;
            Jobs = jobRepository;
            CustomersRelativeRelationships = customerRelativeRelationshipRepository;
            Ransoms = ransomRepository;
            Permission= permissionReporsitory;
            UserPermissionGroup= userPermissionGroupRepository;
            Money= moneyRepository;
            LogContracts = logContractRepository;
        }

     
        public int Save()
        {
            return _dbContext.SaveChanges();
        }
        public Task<int> SaveList()
        {
            return _dbContext.SaveChangesAsync();
        }
        public void Dispose()
          {
            Dispose(true);
            GC.SuppressFinalize(this);
          }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

    }
}
