using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LogContractService : ILogContractService
    {
        private IUnitOfWork _unitOfWork;
        private ILogContractRepository _logContractRepository;
        private DbContextClass _dbContextClass;
        private IServiceProvider _serviceProvider;

        public LogContractService(IUnitOfWork unitOfWork, ILogContractRepository logContractRepository, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
            _logContractRepository = logContractRepository;
            _dbContextClass = dbContextClass;
        }
        public async Task<bool> CreateLogContract(LogContract logContract)
        {
            if (logContract != null)
            {
                logContract.LogTime = DateTime.Now;
                await _unitOfWork.LogContracts.Add(logContract);

                var result = _unitOfWork.Save();
                return (result > 0) ? true : false;
            }
            return false;
        }

        public async Task<IEnumerable<LogContract>> GetLogContracts(int num)
        {
            var logContractList = await _unitOfWork.LogContracts.GetAll();
            if (num == 0)
            {
                return logContractList;
            }
            var result = await _unitOfWork.LogContracts.TakePage(num, logContractList);
            return result;
        }

        public async Task<IEnumerable<LogContract>> LogContractsByBranchId(int branchId)
        {
            var logContract = await _logContractRepository.getLogContractsByBranchId(branchId);
            try
            {
            }
            catch (NullReferenceException e)
            {
                logContract = null;
            }
            return logContract;
        }

        public Task<IEnumerable<LogContract>> LogContractsByContractId(int contractId)
        {
            var logContract = _logContractRepository.getLogContractsByContractId(contractId);
            try
            {
            } catch (NullReferenceException e)
            {
                logContract = null;
            }
            return logContract;
        }
    }
}
