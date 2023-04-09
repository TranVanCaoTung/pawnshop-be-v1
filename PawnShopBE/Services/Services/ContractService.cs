using AutoMapper.Execution;
using Google.Protobuf.WellKnownTypes;
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
        public IUnitOfWork _unitOfWork;
        public IPackageService _iPackageService;
        public IInteresDiaryService _iInterestDiaryService;
        public IContractRepository _iContractRepository;
        private IServiceProvider _serviceProvider;
        private ILedgerService _ledgerService;
        private IContractAssetService _iContractAssetService;
        private IBranchService _branchService;
        private IRansomService _ransomService;
        private ICustomerService _customerService;
        private IPawnableProductService _pawnableService;
        private IWareHouseService _warehouseService;
        private ILogContractService _logContractService;
        private DbContextClass _dbContextClass;
        public ContractService(IUnitOfWork unitOfWork, IContractRepository iContractRepository,
            IContractAssetService contractAssetService, IPackageService packageService,
            IInteresDiaryService interesDiaryService, IServiceProvider serviceProvider,
            ILedgerService ledger, IPawnableProductService pawnable, IWareHouseService wareHouse, DbContextClass dbContextClass, ILogContractService logContractService)
        {
            _unitOfWork = unitOfWork;
            _iContractRepository = iContractRepository;
            _iContractAssetService = contractAssetService;
            _iPackageService = packageService;
            _iInterestDiaryService = interesDiaryService;
            _serviceProvider = serviceProvider;
            _ledgerService = ledger;
            _pawnableService = pawnable;
            _warehouseService = wareHouse;
            _dbContextClass = dbContextClass;
            _logContractService = logContractService;
        }
        private void getParameter()
        {
            _branchService = _serviceProvider.GetService(typeof(IBranchService)) as IBranchService;
            _ransomService = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
            _customerService = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
        }
        
        public async Task exporteExcel()
        {
            var listContract = await GetAllDisplayContracts(0);
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
            getParameter();
            //get all List
            var branchList = await _branchService.GetAllBranch(0);
            var ledgerList = await _ledgerService.GetLedger();
            var contractCollection = await GetAllContracts(0);
            var ransomList = await _ransomService.GetRansom();
            //var customerList = await _customerService.GetAllCustomer(0);
            //var assetList = await _iContractAssetService.GetAllContractAssets();
            //var pawnableList = await _pawnableService.GetAllPawnableProducts(0);
            //var wareHouseList = await _warehouseService.GetWareHouse(0);
            // khai báo filed hiển thị chung 
            var contractList = from c in contractCollection where c.BranchId == branchId select c;
            decimal fund = 0;
            decimal loanLedger = 0;
            decimal recveivedInterest = 0;
            decimal totalProfit = 0;
            decimal ransomTotal = 0;
            //get displayHomePage
            // List<DisplayContractHomePage> listDipslay = new List<DisplayContractHomePage>();
            foreach (var contract in contractList)
            {
                var contractId = contract.ContractId;
                //display.contractCode = contract.ContractCode;
                //display.customerName = getCustomerName(customerList, contract.CustomerId);
                //display.assestCode = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 1);
                //display.assetName = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 2);
                //display.loanContract = contract.Loan;
                //display.startDate = contract.ContractStartDate;
                //display.endDate = contract.ContractEndDate;
                //display.wareName = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 3);
                //display.status = contract.Status;
                //get field hiển thị chung
                if (fund == 0)
                {
                    fund = decimal.Parse(getBranchName(contract.BranchId, branchList, false));
                }
                totalProfit += contract.TotalProfit;
                loanLedger += getLedger(ledgerList, contract.BranchId, true);
                recveivedInterest += getLedger(ledgerList, contract.BranchId, false);
                //add list
                //listDipslay.Add(display);
            }
            var endContractList = from c in contractCollection where c.BranchId == branchId && c.Status != (int)ContractConst.CLOSE select c;
            foreach (var endContract in endContractList)
            {
                ransomTotal += getRansom(ransomList, endContract.ContractId);
            }

            var openContractList = from c in _dbContextClass.Contract
                               where c.BranchId == branchId && c.Status != (int)ContractConst.CLOSE
                               select c;
            var count = openContractList.Count();
            
            DisplayContractHomePage x = new DisplayContractHomePage();
            //add field hiển thị chung
            x.totalProfit = totalProfit;
            x.loanLedger = loanLedger;
            x.fund = fund;
            x.recveivedInterest = recveivedInterest;
            x.ransomTotal = ransomTotal;
            x.openContract = count;
            return x;
        }
        private async Task<IEnumerable<DisplayContractHomePage>> TakePage
            (int number, IEnumerable<DisplayContractHomePage> list)
        {
            var numPage = (int)NumberPage.numPage;
            var skip = (numPage * number) - numPage;
            return list.Skip(skip).Take(numPage);
        }
        private decimal getRansom(IEnumerable<Ransom> ransomList, int contractId)
        {
            var ransom = (from r in ransomList where r.ContractId == contractId select r).FirstOrDefault();
            return ransom.TotalPay;
        }

        private decimal getLedger(IEnumerable<Ledger> ledgerList, int branchId, bool v)
        {
            var ledger = (from l in ledgerList where l.BranchId == branchId select l).FirstOrDefault();
            // true => get loan, false => get receiveInterest (lãi đã nhận)
            if (v)
                return ledger.Loan;
            else
                return ledger.RecveivedInterest;
        }

        private string getBranchName(int branchId, IEnumerable<Branch> branchList, bool v)
        {
            var branch = (from b in branchList where b.BranchId == branchId select b).FirstOrDefault();
            // true => get branch Name, false => get fund
            if (v)
                return branch.BranchName;
            else
                return branch.Fund.ToString();
        }

        private string getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList,
            IEnumerable<PawnableProduct> pawnableList, IEnumerable<Warehouse> warehouseList, int num)
        {
            //get list contract asset
            var asset =(from a in assetList where a.ContractAssetId == contractAssetId select a).FirstOrDefault();
            // 1 => get code Asset, 2 => get name Asset, 3 => get ware name
            switch (num)
            {
                case 1:
                    var pawnable = (from p in pawnableList where
                                          p.PawnableProductId == asset.PawnableProductId select p).FirstOrDefault();
                    return pawnable.CommodityCode;

                case 2:
                    return asset.ContractAssetName;

                case 3:
                    var wareHouse = (from w in warehouseList where w.WarehouseId == asset.WarehouseId select w).FirstOrDefault();
                    return wareHouse.WarehouseName;
            }
            return null;
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
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
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
                    // Create Log Contract
                    var logContract = new LogContract();
                    logContract.ContractId = contract.ContractId;
                    logContract.CustomerName = GetCustomerName(contract.CustomerId);
                    logContract.UserName = GetUser(contract.UserId);
                    logContract.Debt = contract.Loan;
                    logContract.Paid = 0;
                    logContract.LogTime = DateTime.Now;
                    logContract.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
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
            var result = await _unitOfWork.Contracts.TakePage(num, contractList);
            return result;
        }

        public async Task<ICollection<DisplayContractList>> GetAllDisplayContracts(int num)
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
                                                    Status = contract.Status
                                                };

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

                    if (result > 0)
                        return true;
                    else
                        return false;
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
                contractDetail.PackageInterest = package.PackageInterest;
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

        public async Task<bool> CreateContractExpiration(int contractId, string proofImg)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);

            if (contract == null)
            {
                return false;
            }
            using var transaction = await _dbContextClass.Database.BeginTransactionAsync();
            try
            {
                contract.ContractId = 0;
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
                var package = await _unitOfWork.Packages.GetById(contract.PackageId);

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
                var createContractresult = _unitOfWork.Save();
                if (createContractresult == 0)
                {
                    throw new Exception("Failed to create contract.");
                    return false;
                }
                var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;

                await ransomProvider.CreateRansom(contract);
                await interestProvider.CreateInterestDiary(contract);

                // Change Status of old contract into CLOSE
                var oldContract = await _unitOfWork.Contracts.GetById(contractId);
                oldContract.Status = (int)ContractConst.CLOSE;
                oldContract.ActualEndDate = DateTime.Now;

                var oldRansom = await ransomProvider.GetRansomByContractId(contractId);
                oldRansom.PaidMoney = oldRansom.TotalPay;
                oldRansom.Status = (int)RansomConsts.ON_TIME;

                // Close Log Contract
                var contractJoinUserJoinCustomer = from getcontract in _dbContextClass.Contract
                                                   join customer in _dbContextClass.Customer
                                                   on oldContract.CustomerId equals customer.CustomerId
                                                   join user in _dbContextClass.User
                                                   on contract.UserId equals user.UserId
                                                   select new
                                                   {
                                                       ContractId = oldContract.ContractId,
                                                       UserName = user.FullName,
                                                       CustomerName = customer.FullName,
                                                   };
                var oldLogContract = new LogContract();
                foreach (var row in contractJoinUserJoinCustomer)
                {
                    oldLogContract.ContractId = row.ContractId;
                    oldLogContract.UserName = row.UserName;
                    oldLogContract.CustomerName = row.CustomerName;
                }
                oldLogContract.Debt = oldContract.Loan;
                oldLogContract.Paid = oldContract.Loan;
                oldLogContract.LogTime = DateTime.Now;
                oldLogContract.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                oldLogContract.EventType = (int)LogContractConst.CLOSE_CONTRACT;
                await _logContractService.CreateLogContract(oldLogContract);

                // Create Log Contract
                var logContract = new LogContract();
                logContract.ContractId = contract.ContractId;
                logContract.CustomerName = GetCustomerName(contract.CustomerId);
                logContract.UserName = GetUser(contract.UserId);
                logContract.Debt = contract.Loan;
                logContract.Paid = 0;
                logContract.LogTime = DateTime.Now;
                logContract.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                logContract.EventType = (int)LogContractConst.CREATE_CONTRACT;
                await _logContractService.CreateLogContract(logContract);

                // Close Ransom
                var closeRansom = await ransomProvider.GetRansomByContractId(oldContract.ContractId);
                await ransomProvider.SaveRansom(closeRansom.RansomId, proofImg);
                _unitOfWork.Contracts.Update(oldContract);
                _unitOfWork.Save();

                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return false;
            }
        }
        
         

    public async Task<Contract> GetContractByContractCode(string contractCode)
        {
            var contract = await _iContractRepository.getContractByContractCode(contractCode);
            try
            {
                return contract;
            }
            catch (Exception e)
            {
                contract = null;           
            }
            return contract;
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
                    }
                    decimal totalPaid = 0;
                    List<InterestDiary> interestDiaries = (List<InterestDiary>)await _iInterestDiaryService.GetInteresDiariesByContractId(contractId);
                    foreach (var interest in interestDiaries)
                    {
                        totalPaid = totalPaid + interest.PaidMoney;
                    }
                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    var ransom = await ransomProvider.GetRansomByContractId(contractId);
                    totalPaid = totalPaid + ransom.PaidMoney;
                    displayContractInfo.TotalRecived = totalPaid;
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

            // Notification for interest payment
            var diaryProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;

            var contractJoinCustomerJoinAsset = from contract in _dbContextClass.Contract
                                                join customer in _dbContextClass.Customer
                                                on contract.CustomerId equals customer.CustomerId
                                                join contractAsset in _dbContextClass.ContractAsset
                                                on contract.ContractAssetId equals contractAsset.ContractAssetId
                                                join pawnableProduct in _dbContextClass.PawnableProduct
                                                on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                                                join ransom in _dbContextClass.Ransom
                                                on contract.ContractId equals ransom.ContractId
                                                where (contract.BranchId == branchId)
                                                select new
                                                {
                                                    ContractId = contract.ContractId,
                                                    ContractCode = contract.ContractCode,
                                                    CustomerName = customer.FullName,
                                                    CommodityCode = pawnableProduct.CommodityCode,
                                                    ContractAssetName = contractAsset.ContractAssetName,
                                                    RansomTotalPay = ransom.TotalPay,
                                                    ContractStartDate = contract.ContractStartDate,
                                                    ContractEndDate = contract.ContractEndDate,
                                                    Status = contract.Status
                                                };

            foreach (var row in contractJoinCustomerJoinAsset)
            {
                // Notification for ransom (contract is on due date) payment
                if (row.ContractEndDate == DateTime.Today && row.Status != (int)ContractConst.CLOSE)
                {
                    DisplayNotification displayNotification = new DisplayNotification();
                    displayNotification.ContractId = row.ContractId;
                    displayNotification.ContractCode = row.ContractCode;
                    displayNotification.CustomerName = row.CustomerName;
                    displayNotification.CommodityCode = row.CommodityCode;
                    displayNotification.ContractAssetName = row.ContractAssetName;
                    displayNotification.TotalPay = row.RansomTotalPay;
                    displayNotification.ContractStartDate = row.ContractStartDate;
                    displayNotification.ContractEndDate = row.ContractEndDate;
                    displayNotification.Description = "Hợp đồng đến hạn cần thanh toán: " + displayNotification.TotalPay.ToString() + " VND";
                    notifiList.Add(displayNotification);
                }            
            }

            var contractJoinCustomerJoinAssetJoinDiaries = from contract in _dbContextClass.Contract
                                                           join customer in _dbContextClass.Customer
                                                           on contract.CustomerId equals customer.CustomerId
                                                           join contractAsset in _dbContextClass.ContractAsset
                                                           on contract.ContractAssetId equals contractAsset.ContractAssetId
                                                           join pawnableProduct in _dbContextClass.PawnableProduct
                                                           on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                                                           join interestDiary in _dbContextClass.InterestDiary
                                                           on contract.ContractId equals interestDiary.ContractId
                                                           where (contract.BranchId == branchId)
                                                           select new
                                                           {
                                                               ContractId = contract.ContractId,
                                                               ContractCode = contract.ContractCode,
                                                               CustomerName = customer.FullName,
                                                               CommodityCode = pawnableProduct.CommodityCode,
                                                               ContractAssetName = contractAsset.ContractAssetName,
                                                               InterestTotalPay = interestDiary.TotalPay,
                                                               NextDueDate = interestDiary.NextDueDate,
                                                               DueDate = interestDiary.DueDate,
                                                               Status = contract.Status
                                                           };
            foreach (var rows in contractJoinCustomerJoinAssetJoinDiaries)
            {


                if (rows.NextDueDate != DateTime.Today)
                {
                    continue;
                }
                DisplayNotification displayNotification = new DisplayNotification();
                displayNotification.ContractId = rows.ContractId;
                displayNotification.ContractCode = rows.ContractCode;
                displayNotification.CustomerName = rows.CustomerName;
                displayNotification.CommodityCode = rows.CommodityCode;
                displayNotification.ContractAssetName = rows.ContractAssetName;
                displayNotification.TotalPay = rows.InterestTotalPay;
                displayNotification.ContractStartDate = rows.DueDate;
                displayNotification.ContractEndDate = rows.NextDueDate;
                displayNotification.Description = "Kỳ hạn đóng lãi đến cần thanh toán: " + displayNotification.TotalPay.ToString() + " VND";
                notifiList.Add(displayNotification);
            }






            return notifiList;
        }
    }
}
