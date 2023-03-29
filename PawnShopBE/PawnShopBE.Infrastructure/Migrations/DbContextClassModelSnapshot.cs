﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PawnShopBE.Infrastructure.Helpers;

#nullable disable

namespace PawnShopBE.Infrastructure.Migrations
{
    [DbContext(typeof(DbContextClass))]
    partial class DbContextClassModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PawnShopBE.Core.Models.Admin", b =>
                {
                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Admin", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Attribute", b =>
                {
                    b.Property<int>("AttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttributeId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PawnableProductId")
                        .HasColumnType("int");

                    b.HasKey("AttributeId");

                    b.HasIndex("PawnableProductId");

                    b.ToTable("Attribute", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fund")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BranchId");

                    b.ToTable("Branch", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Contract", b =>
                {
                    b.Property<int>("ContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractId"));

                    b.Property<DateTime?>("ActualEndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<int>("ContractAssetId")
                        .HasColumnType("int");

                    b.Property<string>("ContractCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ContractEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ContractStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ContractVerifyImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerVerifyImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("InsuranceFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InterestRecommend")
                        .HasColumnType("int");

                    b.Property<decimal>("Loan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("StorageFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalProfit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ContractId");

                    b.HasIndex("BranchId");

                    b.HasIndex("ContractAssetId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("Contract", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.ContractAsset", b =>
                {
                    b.Property<int>("ContractAssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractAssetId"));

                    b.Property<string>("ContractAssetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PawnableProductId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("ContractAssetId");

                    b.HasIndex("PawnableProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("ContractAsset", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KycId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomerId");

                    b.HasIndex("KycId")
                        .IsUnique();

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.CustomerRelativeRelationship", b =>
                {
                    b.Property<Guid>("CustomerRelativeRelationshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RelativeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelativePhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelativeRelationship")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CustomerRelativeRelationshipId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerRelativeRelationship", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.DependentPeople", b =>
                {
                    b.Property<Guid>("DependentPeopleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerRelationShip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DependentPeopleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("MoneyProvided")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DependentPeopleId");

                    b.HasIndex("CustomerId");

                    b.ToTable("DependentPeople", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.InterestDiary", b =>
                {
                    b.Property<int?>("InterestDiaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("InterestDiaryId"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("InterestDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("NextDueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PaidMoney")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Penalty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProofImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPay")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("InterestDiaryId");

                    b.HasIndex("ContractId");

                    b.ToTable("InterestDiary", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobId"));

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsWork")
                        .HasColumnType("bit");

                    b.Property<string>("LaborContract")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameJob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("WorkLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Job", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Kyc", b =>
                {
                    b.Property<int>("KycId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KycId"));

                    b.Property<string>("FaceImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityCardBacking")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityCardFronting")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KycId");

                    b.ToTable("Kyc", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Ledger", b =>
                {
                    b.Property<int>("LedgerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LedgerId"));

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Loan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ReceivedPrincipal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RecveivedInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("LedgerId");

                    b.HasIndex("BranchId");

                    b.ToTable("Ledger", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Liquidtation", b =>
                {
                    b.Property<int>("LiquidationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LiquidationId"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("LiquidationMoney")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("liquidationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("LiquidationId");

                    b.HasIndex("ContractId")
                        .IsUnique();

                    b.ToTable("Liquidtation", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.LogContract", b =>
                {
                    b.Property<int>("LogContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogContractId"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogContractId");

                    b.HasIndex("ContractId");

                    b.ToTable("LogContract", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackageId"));

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Limitation")
                        .HasColumnType("int");

                    b.Property<int>("LiquitationDay")
                        .HasColumnType("int");

                    b.Property<int>("PackageInterest")
                        .HasColumnType("int");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentPeriod")
                        .HasColumnType("int");

                    b.Property<int>("PunishDay1")
                        .HasColumnType("int");

                    b.Property<int>("PunishDay2")
                        .HasColumnType("int");

                    b.HasKey("PackageId");

                    b.ToTable("Package", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.PawnableProduct", b =>
                {
                    b.Property<int>("PawnableProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PawnableProductId"));

                    b.Property<string>("CommodityCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TypeOfProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PawnableProductId");

                    b.ToTable("PawnableProduct", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Permission", b =>
                {
                    b.Property<int>("perId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("perId"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("namePermission")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("perId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Ransom", b =>
                {
                    b.Property<int>("RansomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RansomId"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PaidMoney")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Penalty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProofImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPay")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RansomId");

                    b.HasIndex("ContractId")
                        .IsUnique();

                    b.ToTable("Ransom", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.RefeshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefeshToken");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("BranchId");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.UserPermissionGroup", b =>
                {
                    b.Property<int>("perId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("perId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissionGroups");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WarehouseId"));

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("WarehouseAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WarehouseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WarehouseId");

                    b.ToTable("Warehouse", (string)null);
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Attribute", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.PawnableProduct", "PawnableProduct")
                        .WithMany("Attributes")
                        .HasForeignKey("PawnableProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PawnableProduct");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Contract", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Branch", "Branch")
                        .WithMany("Contracts")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.ContractAsset", "ContractAsset")
                        .WithMany("Contracts")
                        .HasForeignKey("ContractAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.Customer", "Customer")
                        .WithMany("Contracts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.Package", "Package")
                        .WithMany("Contracts")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.User", "User")
                        .WithMany("Contracts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("ContractAsset");

                    b.Navigation("Customer");

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.ContractAsset", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.PawnableProduct", "PawnableProduct")
                        .WithMany("ContractAssets")
                        .HasForeignKey("PawnableProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.Warehouse", "Warehouse")
                        .WithMany("ContractAssets")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PawnableProduct");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Customer", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Kyc", "Kyc")
                        .WithOne("Customer")
                        .HasForeignKey("PawnShopBE.Core.Models.Customer", "KycId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kyc");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.CustomerRelativeRelationship", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Customer", "Customer")
                        .WithMany("CustomerRelativeRelationships")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.DependentPeople", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Customer", "Customer")
                        .WithMany("DependentPeople")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.InterestDiary", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Contract", "Contract")
                        .WithMany("InterestDiaries")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Job", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Customer", "Customer")
                        .WithMany("Jobs")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Ledger", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Branch", "Branch")
                        .WithMany("Ledgers")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Liquidtation", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Contract", "Contract")
                        .WithOne("Liquidtation")
                        .HasForeignKey("PawnShopBE.Core.Models.Liquidtation", "ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.LogContract", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Contract", "Contract")
                        .WithMany("LogContracts")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Ransom", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Contract", "Contract")
                        .WithOne("Ransom")
                        .HasForeignKey("PawnShopBE.Core.Models.Ransom", "ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.User", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.Branch", "Branch")
                        .WithMany("Users")
                        .HasForeignKey("BranchId");

                    b.HasOne("PawnShopBE.Core.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.UserPermissionGroup", b =>
                {
                    b.HasOne("PawnShopBE.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnShopBE.Core.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("perId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Branch", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Ledgers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Contract", b =>
                {
                    b.Navigation("InterestDiaries");

                    b.Navigation("Liquidtation");

                    b.Navigation("LogContracts");

                    b.Navigation("Ransom");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.ContractAsset", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Customer", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("CustomerRelativeRelationships");

                    b.Navigation("DependentPeople");

                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Kyc", b =>
                {
                    b.Navigation("Customer")
                        .IsRequired();
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Package", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.PawnableProduct", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("ContractAssets");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.User", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("PawnShopBE.Core.Models.Warehouse", b =>
                {
                    b.Navigation("ContractAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
