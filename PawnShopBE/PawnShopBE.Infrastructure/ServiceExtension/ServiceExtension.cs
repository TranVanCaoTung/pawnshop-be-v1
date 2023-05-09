using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContextClass>(options =>
            {
                object value = options.UseSqlServer(configuration.GetConnectionString("PawnShop"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IRoleRepository,RoleRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractAssetRepository, ContractAssetRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<ICustomerRelativeRelationshipRepository, CustomerRelativeRelationshipRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDependentPeopleRepository, DependentPeopleRepository>();
            services.AddScoped<IInterestDiaryRepository, InterestDiaryRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IKycRepository, KycRepository>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<ILiquidationRepository, LiquidationRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPawnableProductRepository, PawnableProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IRansomRepository, RansomRepository>();
            services.AddScoped<IPermissionReporsitory,PermissionRepository>();
            services.AddScoped<IUserPermissionGroupRepository, UserPerGroupRepository>();
            services.AddScoped<ILogContractRepository, LogContractRepository>();
            services.AddScoped<IDiaryImgRepository, DiaryImgRepository>();
            services.AddScoped<ILogAssetRepository, LogAssetRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserBranchRepository, UserBranchRepository>();
            
            return services;
        }
    }
}
