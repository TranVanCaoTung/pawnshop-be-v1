using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace PawnShopBE.Infrastructure.Helpers
{
    public class DbContextClass : DbContext
    {
        public DbContextClass()
        {
        }

        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {

        }
        #region DbSet
        public DbSet<User> User { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RefeshToken> RefeshTokens { get; set; }
        public DbSet<Ledger> Ledger { get; set; }
        public DbSet<InterestDiary> InterestDiary { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Liquidtation> Liquidtation { get; set; }
        public DbSet<PawnableProduct> PawnableProduct { get; set; }
        public DbSet<ContractAsset> ContractAsset { get; set; }
        public DbSet<Attribute> Attribute { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<DependentPeople> DependentPeople { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<CustomerRelativeRelationship> CustomerRelativeRelationship { get; set; }
        public DbSet<Kyc> Kyc { get; set; }
        public DbSet<Ransom> Ransom { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermissionGroup> UserPermissionGroups { get; set; }
        public DbSet<LogContract> LogContracts { get; set; }
        public DbSet<LogAsset> LogAssets { get; set; }
        public DbSet<DiaryImg> DiaryImgs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
            modelBuilder.Entity<User>(entity =>
            {
                // Table mapping to db
                entity.ToTable("User");
                // PK
                entity.HasKey(u => u.UserId);
                // Relative
                entity.HasOne(u => u.Branch).WithMany(b => b.Users).HasForeignKey(u => u.BranchId).IsRequired(false);
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId).IsRequired(true);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Branch");
                entity.HasKey(b => b.BranchId);
                entity.HasMany(b => b.Ledgers).WithOne(l => l.Branch);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(r => r.RoleId);
            });

            modelBuilder.Entity<Ledger>(entity =>
            {
                entity.ToTable("Ledger");
                entity.HasKey(l => l.LedgerId);
                entity.HasOne(l => l.Branch).WithMany(u => u.Ledgers).HasForeignKey(l => l.BranchId).IsRequired(true);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");
                entity.HasKey(c => c.ContractId);
                entity.HasOne(c => c.Customer).WithMany(cus => cus.Contracts).HasForeignKey(c => c.CustomerId).IsRequired(true);
                entity.HasOne(c => c.Package).WithMany(p => p.Contracts).HasForeignKey(c => c.PackageId).IsRequired(true);
                entity.HasOne(c => c.Branch).WithMany(p => p.Contracts).HasForeignKey(c => c.BranchId).IsRequired(true);
                entity.HasOne(c => c.ContractAsset).WithMany(p => p.Contracts).HasForeignKey(c => c.ContractAssetId).IsRequired(true);
                entity.HasOne(c => c.User).WithMany(p => p.Contracts).HasForeignKey(c => c.UserId).IsRequired(true);
            });

            modelBuilder.Entity<InterestDiary>(entity =>
            {
                entity.ToTable("InterestDiary");
                entity.HasKey(i => i.InterestDiaryId);
                entity.HasOne(i => i.Contract).WithMany(c => c.InterestDiaries).HasForeignKey(i => i.ContractId).IsRequired(true);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable("Package");
                entity.HasKey(p => p.PackageId);
            });

            modelBuilder.Entity<Liquidtation>(entity =>
            {
                entity.ToTable("Liquidtation");
                entity.HasKey(li => li.LiquidationId);
                entity.HasOne(li => li.Contract).WithOne(c => c.Liquidtation).HasForeignKey<Liquidtation>(li => li.ContractId);
            });

            modelBuilder.Entity<ContractAsset>(entity =>
            {
                entity.ToTable("ContractAsset");
                entity.HasKey(c => c.ContractAssetId);
                entity.HasOne(c => c.PawnableProduct).WithMany(p => p.ContractAssets).HasForeignKey(c => c.PawnableProductId).IsRequired(true);
                entity.HasOne(c => c.Warehouse).WithMany(p => p.ContractAssets).HasForeignKey(c => c.WarehouseId).IsRequired(true);
            });

            modelBuilder.Entity<PawnableProduct>(entity =>
            {
                entity.ToTable("PawnableProduct");
                entity.HasKey(p => p.PawnableProductId);
            });

            modelBuilder.Entity<Attribute>(entity =>
            {
                entity.ToTable("Attribute");
                entity.HasKey(a => a.AttributeId);
                entity.HasOne(a => a.PawnableProduct).WithMany(p => p.Attributes).HasForeignKey(a => a.PawnableProductId).IsRequired(true);

            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouse");
                entity.HasKey(w => w.WarehouseId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(c => c.CustomerId);
                entity.HasOne(c => c.Kyc).WithOne(k => k.Customer).HasForeignKey<Customer>(c => c.KycId).IsRequired(true);
            });

            modelBuilder.Entity<DependentPeople>(entity =>
            {
                entity.ToTable("DependentPeople");
                entity.HasKey(d => d.DependentPeopleId);
                entity.HasOne(d => d.Customer).WithMany(c => c.DependentPeople).HasForeignKey(d => d.CustomerId).IsRequired(true);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");
                entity.HasKey(j => j.JobId);
                entity.HasOne(j => j.Customer).WithMany(c => c.Jobs).HasForeignKey(d => d.CustomerId).IsRequired(true);
            });

            modelBuilder.Entity<CustomerRelativeRelationship>(entity =>
            {
                entity.ToTable("CustomerRelativeRelationship");
                entity.HasKey(j => j.CustomerRelativeRelationshipId);
                entity.HasOne(j => j.Customer).WithMany(c => c.CustomerRelativeRelationships).HasForeignKey(d => d.CustomerId).IsRequired(true);
            });

            modelBuilder.Entity<Kyc>(entity => 
            {
                entity.ToTable("Kyc");
                entity.HasKey("KycId");
            });
            modelBuilder.Entity<Ransom>(entity =>
            {
                entity.ToTable("Ransom");
                entity.HasKey(r => r.RansomId);
                entity.HasOne(r => r.Contract).WithOne(c => c.Ransom).HasForeignKey<Ransom>(r => r.ContractId).IsRequired(true);

            });
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");
                entity.HasKey(a => a.Id);
            });
            modelBuilder.Entity<UserPermissionGroup>(entity =>
            {
                entity.HasKey(e => new{e.perId, e.UserId});
            });
            modelBuilder.Entity<LogContract>(entity =>
            {
                entity.ToTable("LogContract");
                entity.HasKey(l => l.LogContractId);
                entity.HasOne(l => l.Contract).WithMany(c => c.LogContracts).HasForeignKey(l => l.ContractId).IsRequired(true);
            });
            modelBuilder.Entity<LogAsset>(entity =>
            {
                entity.ToTable("LogAsset");
                entity.HasKey(l => l.logAssetId);
                entity.HasOne(l => l.ContractAsset).WithMany(c => c.LogAssets).HasForeignKey(l => l.contractAssetId).IsRequired(true);
            });
            modelBuilder.Entity<DiaryImg>(entity =>
            {
                entity.ToTable("DiaryImg");
                entity.HasKey(d => d.DiaryImgId);
                entity.HasOne(l => l.InterestDiary).WithMany(c => c.DiaryImgs).HasForeignKey(l => l.InterestDiaryId).IsRequired(true);
            });
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(n => n.NotificationId);
                entity.HasOne(b => b.Branch).WithMany(n => n.Notifications).HasForeignKey(n => n.BranchId).IsRequired(true);
            });
        }
    } 
}
