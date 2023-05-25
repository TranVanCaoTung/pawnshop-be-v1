using AutoMapper.Execution;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = PawnShopBE.Core.Models.Contract;
using Excel = Microsoft.Office.Interop.Excel;
namespace Services.Services
{
    public class ContractService : IContractService
    {
        private IUnitOfWork _unitOfWork;
        private IPackageService _iPackageService;
        private IInteresDiaryService _iInterestDiaryService;
        private IContractRepository _iContractRepository;
        private IServiceProvider _serviceProvider;
        private IContractAssetService _iContractAssetService;
        private ILogContractService _logContractService;
        private ILogAssetService _logAssetService;
        private DbContextClass _dbContextClass;
        private IContractAssetService _contractAssetService;
        public ContractService(IUnitOfWork unitOfWork, IContractRepository iContractRepository,
            IContractAssetService contractAssetService, IPackageService packageService,
            IInteresDiaryService interesDiaryService, IServiceProvider serviceProvider, DbContextClass dbContextClass,
            ILogContractService logContractService, ILogAssetService logAssetService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _iContractRepository = iContractRepository;
            _iContractAssetService = contractAssetService;
            _iPackageService = packageService;
            _iInterestDiaryService = interesDiaryService;
            _serviceProvider = serviceProvider;
            _dbContextClass = dbContextClass;
            _logContractService = logContractService;
            _logAssetService = logAssetService;
            _contractAssetService = contractAssetService;
        }
        public async Task exporteExcel(int branchId)
        {
            var listContract = await GetAllDisplayContracts(0, 1);
            //set cấu hình excel
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
            //setting content title
            exSheet.Range["E1"].Font.Size = 20;
            exSheet.Range["E1"].Font.Bold = true;
            exSheet.Range["E1"].Font.Color = Color.Red;
            exSheet.Range["E1"].Value = "HỢP ĐỒNG CẦM ĐỒ";
            //setting thông tin chung
            exSheet.Range["A3:J3"].Font.Size = 16;
            exSheet.Range["A3:J3"].Font.Bold = true;
            exSheet.Range["A3:J3"].Font.Color = Color.DarkBlue;
            //exSheet.Range["A3:J3"].Font.Background = Color.Gray;
            exSheet.Range["A3"].Value = "#";
            exSheet.Range["B3"].Value = "Mã HĐ";
            exSheet.Range["C3"].Value = "Khách Hàng";
            exSheet.Range["D3"].Value = "Mã TS";
            exSheet.Range["E3"].Value = "Tài Sản";
            exSheet.Range["F3"].Value = "Tiền Cầm";
            exSheet.Range["G3"].Value = "Ngày Cầm";
            exSheet.Range["H3"].Value = "Ngày Đến Hạn";
            exSheet.Range["I3"].Value = "Kho";
            exSheet.Range["J3"].Value = "Tình Trạng";
            //Setting column width
            exSheet.Range["B3"].ColumnWidth = 9;
            exSheet.Range["C3"].ColumnWidth = 16;
            exSheet.Range["A3"].ColumnWidth = 4;
            exSheet.Range["D3"].ColumnWidth = 9;
            exSheet.Range["E3"].ColumnWidth = 15;
            exSheet.Range["F3"].ColumnWidth = 12;
            exSheet.Range["G3"].ColumnWidth = 18;
            exSheet.Range["H3"].ColumnWidth = 18;
            exSheet.Range["I3"].ColumnWidth = 9;
            exSheet.Range["J3"].ColumnWidth = 18;
            //--------
            //exSheet.Range["B3"].Columns.AutoFit();
            //exSheet.Range["C3"].Columns.AutoFit();
            //exSheet.Range["A3"].Columns.AutoFit();
            //exSheet.Range["E3"].Columns.AutoFit();
            //exSheet.Range["F3"].Columns.AutoFit();
            //exSheet.Range["G3"].Columns.AutoFit();
            //exSheet.Range["H3"].Columns.AutoFit();
            //exSheet.Range["J3"].Columns.AutoFit();
            //exSheet.Range["I3"].Columns.AutoFit();
            //setting nội dung từng contract
            int number = 1;
            int i = 4;
            foreach (var x in listContract)
            {
                exSheet.Range["A" + i.ToString() + ":J" + i.ToString()].Font.Size = 12;
                exSheet.Range["A" + i.ToString()].Value = number.ToString();
                exSheet.Range["B" + i.ToString()].Value = x.ContractCode;
                exSheet.Range["C" + i.ToString()].Value = x.CustomerName;
                exSheet.Range["D" + i.ToString()].Value = x.CommodityCode;
                exSheet.Range["E" + i.ToString()].Value = x.ContractAssetName;
                exSheet.Range["F" + i.ToString()].Value = x.Loan.ToString();
                exSheet.Range["G" + i.ToString()].Value = x.ContractStartDate.ToString();
                exSheet.Range["H" + i.ToString()].Value = x.ContractEndDate.ToString();
                exSheet.Range["I" + i.ToString()].Value = x.WarehouseName;
                exSheet.Range["J" + i.ToString()].Value = getStatusContract(x.Status);
                i++;
                number++;
            }
            exSheet.Name = "Report";
            exApp.Visible = true;
            exBook.Activate();
            //save Excel
            //exBook.SaveAs("C:\\file.xls", Excel.XlFileFormat.xlWorkbookNormal,
            //    null, null, false, false,
            //    Excel.XlSaveAsAccessMode.xlExclusive,
            //    false, false, false, false, false);
            exApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);
        }

        private string getStatusContract(int status)
        {
            switch (status)
            {
                case 1:
                    return "Đang Tiến Hành";
                case 2:
                    return "Quá Hạn";
                case 3:
                    return "Thanh Lý";
                case 4:
                    return "Đã Đóng";
            }
            return null;
        }

        public async Task<DisplayContractHomePage> getAllContractHomepage(int branchId)
        {
            var branchServiceProvider = _serviceProvider.GetService(typeof(IBranchService)) as IBranchService;
            var contractService = _serviceProvider.GetService(typeof(IContractService)) as IContractService;

            var branch = await branchServiceProvider.GetBranchById(branchId);
            var contractList = await contractService.GetAllContracts();
            contractList = from c in contractList
                           where c.BranchId == branchId
                           select c;

            var displayContractHomePage = new DisplayContractHomePage();
            displayContractHomePage.BranchId = branch.BranchId;
            displayContractHomePage.Fund = branch.Fund;
            displayContractHomePage.OpenContract = contractList.Where(c => c.Status == (int)ContractConst.IN_PROGRESS).Count();
            displayContractHomePage.LateContract = contractList.Where(c => c.Status == (int)ContractConst.OVER_DUE).Count();
            displayContractHomePage.LiquidationContract = contractList.Where(c => c.Status == (int)ContractConst.LIQUIDATION).Count();

            return displayContractHomePage;
        }

        private string GetCustomerName(Guid customerId)
        {
            var customerIenumerable = from c in _dbContextClass.Customer
                                      where c.CustomerId == customerId
                                      select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer.FullName;
        }

        private string GetUser(Guid userId)
        {
            var userIenumerable = from u in _dbContextClass.User
                                  where u.UserId == userId
                                  select u;
            var user = userIenumerable.FirstOrDefault();
            return user.FullName;
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                // Get current index of contracts
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();

                // Get package 
                var package = await _iPackageService.GetPackageById(contract.PackageId);

                if (package != null)
                {
                    var fee = contract.InsuranceFee + contract.StorageFee;
                    var period = package.Day / package.PaymentPeriod;

                    // Use recommend interest if input
                    double interest = (contract.InterestRecommend != 0) ? contract.InterestRecommend * 0.01 : package.PackageInterest * 0.01;
                    contract.TotalProfit = (contract.Loan * (decimal)interest) + (fee * period);
                }
                contract.ContractStartDate = DateTime.Now;
                contract.ContractEndDate = contract.ContractStartDate.AddDays((double)package.Day - 1);
                contract.Status = (int)ContractConst.IN_PROGRESS;
                await _unitOfWork.Contracts.Add(contract);
                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    var contractAsset = _dbContextClass.ContractAsset.FirstOrDefault(w => w.ContractAssetId == contract.ContractAssetId);
                    var warehouse = _dbContextClass.Warehouse.FirstOrDefault(w => w.WarehouseId == contractAsset.WarehouseId);
                    // Create Log Asset
                    var logAsset = new LogAsset();
                    logAsset.contractAssetId = contract.ContractAssetId;
                    logAsset.Description = null;
                    logAsset.ImportImg = null;
                    logAsset.ExportImg = null;
                    logAsset.UserName = GetUser(contract.UserId);
                    logAsset.WareHouseName = warehouse.WarehouseName;
                    logAsset.CreateDate = DateTime.Now;
                    await _logAssetService.CreateLogAsset(logAsset);
                    // Create Log Contract
                    var logContract = new LogContract();
                    logContract.ContractId = contract.ContractId;
                    logContract.CustomerName = GetCustomerName(contract.CustomerId);
                    logContract.UserName = GetUser(contract.UserId);
                    logContract.Debt = contract.Loan;
                    logContract.Paid = 0;
                    logContract.LogTime = DateTime.Now;
                    logContract.Description = "Tạo mới hợp đồng giá trị " + (int)contract.Loan + " VND.";
                    logContract.EventType = (int)LogContractConst.CREATE_CONTRACT;
                    await _logContractService.CreateLogContract(logContract);

                    // Create Ransom
                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    await ransomProvider.CreateRansom(contract);

                    // Create Interest Diary
                    var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;
                    await interestProvider.CreateInterestDiary(contract);
                    return true;
                }
                if (await plusPoint(contract))
                    return true;
            }
            return false;
        }
        private async Task<bool> plusPoint(Contract contract)
        {
            var provider = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
            var customerList = await provider.GetAllCustomer(0);
            var customerIenumerable = (from c in customerList where c.CustomerId == contract.CustomerId select c).FirstOrDefault();
            var customer = new Customer();
            customer = customerIenumerable;
            //plus point
            customer.Point += 100;
            if (await provider.UpdateCustomer(customer))
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    _unitOfWork.Contracts.Delete(contract);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<IEnumerable<Contract>> GetAllContracts(int num)
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (num == 0)
            {
                return contractList;
            }
            contractList.OrderByDescending(c => c.ContractStartDate);
            var result = await _unitOfWork.Contracts.TakePage(num, contractList);
            return result;
        }

        public async Task<ICollection<DisplayContractList>> GetAllDisplayContracts(int num, int branchId)
        {
            var contractJoinCustomerJoinAsset = from contract in _dbContextClass.Contract
                                                join customer in _dbContextClass.Customer
                                                on contract.CustomerId equals customer.CustomerId
                                                join contractAsset in _dbContextClass.ContractAsset
                                                on contract.ContractAssetId equals contractAsset.ContractAssetId
                                                join pawnableProduct in _dbContextClass.PawnableProduct
                                                on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                                                join warehouse in _dbContextClass.Warehouse
                                                on contractAsset.WarehouseId equals warehouse.WarehouseId
                                                where contract.BranchId == branchId
                                                select new
                                                {
                                                    ContractId = contract.ContractId,
                                                    ContractCode = contract.ContractCode,
                                                    CustomerName = customer.FullName,
                                                    CommodityCode = pawnableProduct.CommodityCode,
                                                    ContractAssetName = contractAsset.ContractAssetName,
                                                    ContractLoan = contract.Loan,
                                                    ContractStartDate = contract.ContractStartDate,
                                                    ContractEndDate = contract.ContractEndDate,
                                                    WarehouseName = warehouse.WarehouseName,
                                                    Status = contract.Status,
                                                    CCCD = customer.CCCD
                                                };
            contractJoinCustomerJoinAsset = contractJoinCustomerJoinAsset.OrderByDescending(c => c.ContractId);
            List<DisplayContractList> displayContractList = new List<DisplayContractList>();
            foreach (var row in contractJoinCustomerJoinAsset)
            {
                DisplayContractList displayContract = new DisplayContractList();
                displayContract.ContractId = row.ContractId;
                displayContract.ContractCode = row.ContractCode;
                displayContract.CustomerName = row.CustomerName;
                displayContract.CommodityCode = row.CommodityCode;
                displayContract.ContractAssetName = row.ContractAssetName;
                displayContract.Loan = row.ContractLoan;
                displayContract.ContractStartDate = row.ContractStartDate;
                displayContract.ContractEndDate = row.ContractEndDate;
                displayContract.WarehouseName = row.WarehouseName;
                displayContract.Status = row.Status;
                displayContract.CCCD = row.CCCD;
                displayContractList.Add(displayContract);
            }
            List<DisplayContractList> result = await _iContractRepository.displayContractListTakePage(num, displayContractList);
            if (num == 0)
            {
                return displayContractList;
            }
            return result;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (contractList != null)
            {
                return contractList;

            }
            return null;
        }

        public async Task<Contract> GetContractById(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    return contract;
                }
            }
            return null;
        }

        public async Task<bool> UpdateContract(int contractId, Contract contract)
        {
            if (contract != null)
            {
                var contractUpdate = await _unitOfWork.Contracts.GetById(contractId);
                if (contractUpdate != null)
                {
                    // Update Asset
                    if (contract.ContractAsset != null)
                    {
                        var assetUpdate = await _iContractAssetService.UpdateContractAsset(contract.ContractAsset);
                    }
                    contractUpdate.CustomerVerifyImg = contract.CustomerVerifyImg;
                    contractUpdate.ContractVerifyImg = contract.ContractVerifyImg;
                    contractUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Contracts.Update(contractUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0) return true;
                }
            }
            return false;
        }
        public async Task<DisplayContractDetail> GetContractDetail(int contractId)
        {
            var contractDetail = new DisplayContractDetail();
            if (contractId == null)
            {
                return null;
            }
            try
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);

                var customer = await _unitOfWork.Customers.GetById(contract.CustomerId);
                var package = await _iPackageService.GetPackageById(contract.PackageId);
                List<InterestDiary> interestDiaries = (List<InterestDiary>)await _iInterestDiaryService.GetInteresDiariesByContractId(contractId);

                decimal interestPaid = 0;
                decimal interestDebt = 0;
                foreach (InterestDiary interestDiary in interestDiaries)
                {
                    interestPaid = interestPaid + interestDiary.PaidMoney;
                    interestDebt = interestDebt + interestDiary.InterestDebt;
                }
                contractDetail.CustomerName = customer.FullName;
                contractDetail.Phone = contract.Customer.Phone;
                contractDetail.Loan = contract.Loan;
                contractDetail.ContractStartDate = contract.ContractStartDate;
                contractDetail.ContractEndDate = contract.ContractEndDate;
                contractDetail.PackageInterest = (contract.InterestRecommend != 0) ? contract.InterestRecommend : package.PackageInterest;
                contractDetail.InterestPaid = interestPaid;
                contractDetail.InterestDebt = interestDebt;
                contractDetail.Status = contract.Status;
            }
            catch (Exception e)
            {
                contractDetail = null;
            }
            return contractDetail;
        }

        public async Task<bool> UploadContractImg(int contractId, string? customerImg, string? contractImg)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);
            if (contract != null)
            {
                if (customerImg != null) contract.CustomerVerifyImg = customerImg;
                if (contractImg != null) contract.ContractVerifyImg = contractImg;
            }
            _unitOfWork.Contracts.Update(contract);
            var result = _unitOfWork.Save();

            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> CreateContractExpiration(int contractId, string proofImg, Guid userId)
        {
            var oldContract = await GetContractById(contractId);
            if (oldContract != null)
            {
                var transaction = await _dbContextClass.Database.BeginTransactionAsync();
                try
                {
                    // Create re-asset for contract expiration
                    var oldContractAsset = await _contractAssetService.GetContractAssetById(oldContract.ContractAssetId);
                    var newContractAsset = new ContractAsset();
                    newContractAsset.ContractAssetName = oldContractAsset.ContractAssetName;
                    newContractAsset.Image = oldContractAsset.Image;
                    newContractAsset.Description = oldContractAsset.Description;
                    newContractAsset.PawnableProductId = oldContractAsset.PawnableProductId;
                    newContractAsset.WarehouseId = oldContractAsset.WarehouseId;
                    newContractAsset.Status = (int)ContractAssetConst.IN_STOCK;
                    var assetCreated = await _contractAssetService.CreateContractAsset(newContractAsset);

                    // Update status out_stock for old contract asset
                    oldContractAsset.Status = (int)ContractAssetConst.OUT_STOCK;
                    await _contractAssetService.UpdateContractAsset(oldContractAsset);
                    if (assetCreated)
                    {
                        // Create contract expiration
                        var newContract = new Contract();
                        newContract.UserId = userId;
                        newContract.ContractAssetId = newContractAsset.ContractAssetId;
                        newContract.CustomerId = oldContract.CustomerId;
                        newContract.PackageId = oldContract.PackageId;
                        newContract.BranchId = oldContract.BranchId;
                        newContract.InterestRecommend = oldContract.InterestRecommend;
                        newContract.Loan = oldContract.Loan;
                        newContract.InsuranceFee = oldContract.InsuranceFee;
                        newContract.StorageFee = oldContract.StorageFee;

                        var contractCreated = await CreateContract(newContract);

                        // Change status CLOSE for old contract
                        oldContract.Status = (int)ContractConst.CLOSE;
                        oldContract.ActualEndDate = DateTime.Now;

                        var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                        var oldRansom = await ransomProvider.GetRansomByContractId(contractId);
                        oldRansom.PaidMoney = oldRansom.TotalPay;
                        oldRansom.Status = (int)RansomConsts.ON_TIME;

                        // Close LogContract of old contract
                        var contractJoinUserJoinCustomer = from getcontract in _dbContextClass.Contract
                                                           join customer in _dbContextClass.Customer
                                                           on oldContract.CustomerId equals customer.CustomerId
                                                           join user in _dbContextClass.User
                                                           on oldContract.UserId equals user.UserId
                                                           select new
                                                           {
                                                               ContractId = oldContract.ContractId,
                                                               UserName = user.FullName,
                                                               CustomerName = customer.FullName,
                                                           };
                        // Close Ransom of oldcontract
                        var closeRansom = await ransomProvider.GetRansomByContractId(oldContract.ContractId);
                        await ransomProvider.SaveRansom(closeRansom.RansomId, proofImg);
                        _unitOfWork.Contracts.Update(oldContract);
                        _unitOfWork.Save();

                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }

        public async Task<DisplayContractInfo> GetContractInfoByContractId(int contractId)
        {
            var displayContractInfo = new DisplayContractInfo();
            try
            {
                var contractJoinPackageJoinAssetJoinCustomerJoinUser = from contract in _dbContextClass.Contract
                                                                       join customer in _dbContextClass.Customer
                                                                       on contract.CustomerId equals customer.CustomerId
                                                                       join user in _dbContextClass.User
                                                                       on contract.UserId equals user.UserId
                                                                       join contractAsset in _dbContextClass.ContractAsset
                                                                       on contract.ContractAssetId equals contractAsset.ContractAssetId
                                                                       join pawnableProduct in _dbContextClass.PawnableProduct
                                                                       on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                                                                       join warehouse in _dbContextClass.Warehouse
                                                                       on contractAsset.WarehouseId equals warehouse.WarehouseId
                                                                       join package in _dbContextClass.Package
                                                                       on contract.PackageId equals package.PackageId
                                                                       where contract.ContractId == contractId
                                                                       select new
                                                                       {
                                                                           ContractCode = contract.ContractCode,
                                                                           CustomerName = customer.FullName,
                                                                           CCCD = customer.CCCD,
                                                                           PhoneNumber = customer.Phone,
                                                                           Address = customer.Address,
                                                                           TypeOfProduct = pawnableProduct.TypeOfProduct,
                                                                           ContractAssetName = contractAsset.ContractAssetName,
                                                                           InsuranceFee = contract.InsuranceFee,
                                                                           StorageFee = contract.StorageFee,
                                                                           ContractLoan = contract.Loan,
                                                                           UserName = user.UserName,
                                                                           ContractStartDate = contract.ContractStartDate,
                                                                           Description = contractAsset.Description,
                                                                           AssetImg = contractAsset.Image,
                                                                           PackageName = package.PackageName,
                                                                           PaymentPeriod = package.PaymentPeriod,
                                                                           PackageInterest = package.PackageInterest,
                                                                           InterestRecomend = contract.InterestRecommend,
                                                                           TotalProfit = contract.TotalProfit,
                                                                           WarehouseName = warehouse.WarehouseName,
                                                                           ContractStatus = contract.Status,
                                                                           ContractAssetId = contractAsset.ContractAssetId,
                                                                           WarehouseId = warehouse.WarehouseId
                                                                       };
                if (contractJoinPackageJoinAssetJoinCustomerJoinUser != null)
                {

                    foreach (var row in contractJoinPackageJoinAssetJoinCustomerJoinUser)
                    {
                        displayContractInfo.ContractId = contractId;
                        displayContractInfo.ContractCode = row.ContractCode;
                        displayContractInfo.ContractStartDate = row.ContractStartDate;
                        displayContractInfo.Loan = row.ContractLoan;
                        displayContractInfo.InsuranceFee = row.InsuranceFee;
                        displayContractInfo.StorageFee = row.StorageFee;
                        displayContractInfo.PackageName = row.PackageName;
                        displayContractInfo.PaymentPeriod = row.PaymentPeriod;
                        displayContractInfo.PackageInterest = row.PackageInterest;
                        displayContractInfo.InterestRecommend = row.InterestRecomend;
                        displayContractInfo.CustomerName = row.CustomerName;
                        displayContractInfo.CCCD = row.CCCD;
                        displayContractInfo.PhoneNumber = row.PhoneNumber;
                        displayContractInfo.Address = row.Address;
                        displayContractInfo.TypeOfProduct = row.TypeOfProduct;
                        displayContractInfo.AssetName = row.ContractAssetName;
                        displayContractInfo.WarehouseName = row.WarehouseName;
                        displayContractInfo.UserName = row.UserName;
                        displayContractInfo.AssetImg = row.AssetImg;
                        displayContractInfo.TotalProfit = row.TotalProfit;
                        var attribute = row.Description;
                        string[] attributes = attribute.Split("/");
                        displayContractInfo.AttributeInfos = attributes;
                        displayContractInfo.Status = row.ContractStatus;
                        displayContractInfo.ContractAssetId = row.ContractAssetId;
                        displayContractInfo.WarehouseId = row.WarehouseId;
                    }
                    decimal interestPaid = 0;
                    decimal interestDebt = 0;
                    decimal totalPaid = 0;

                    List<InterestDiary> interestDiaries = (List<InterestDiary>)await _iInterestDiaryService.GetInteresDiariesByContractId(contractId);
                    foreach (var interest in interestDiaries)
                    {
                        totalPaid = totalPaid + interest.PaidMoney;
                        interestPaid = interestPaid + interest.PaidMoney;
                        interestDebt = interestDebt + interest.InterestDebt;
                    }

                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    var ransom = await ransomProvider.GetRansomByContractId(contractId);
                    totalPaid = totalPaid + ransom.PaidMoney;
                    displayContractInfo.TotalRecived = totalPaid;
                    displayContractInfo.InterestDebt = interestDebt;
                    displayContractInfo.InterestPaid = interestPaid;
                }
            }
            catch (Exception e)
            {
                displayContractInfo = null;
            }
            return displayContractInfo;
        }

        public async Task<IEnumerable<DisplayNotification>> NotificationList(int branchId)
        {
            var notifiList = new List<DisplayNotification>();
            var notificationList = await _dbContextClass.Notifications.ToListAsync();
            var contractsByBranchId = await _dbContextClass.Contract.Where(x => x.BranchId == branchId && x.ContractEndDate.Date == DateTime.Now.Date).ToListAsync();

            foreach (var notification in notificationList)
            {
                var dispplayNotifcation = new DisplayNotification();
                foreach (var  contract in contractsByBranchId)
                {
                    dispplayNotifcation.NotificationId = notification.NotificationId;
                    dispplayNotifcation.ContractId = contract.ContractId;
                    dispplayNotifcation.Header = notification.Header;
                    dispplayNotifcation.Content = notification.Content;
                    dispplayNotifcation.Type = notification.Type;
                    dispplayNotifcation.CreatedDate = notification.CreatedDate;
                    dispplayNotifcation.IsRead = notification.IsRead;
                }
                notifiList.Add(dispplayNotifcation);
            }
            return notifiList.OrderByDescending(x => x.CreatedDate);
        }
    }
}
