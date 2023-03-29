using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IContractService
    {
        Task<bool> CreateContract(Contract contract);

        Task<IEnumerable<Contract>> GetAllContracts(int num);

        Task<ICollection<DisplayContractList>> GetAllDisplayContracts(int num);
        Task<IEnumerable<Contract>> GetAllContracts();
        Task<Contract> GetContractById(int contractId);
        Task<Contract> GetContractByContractCode(string contractCode);
        Task<DisplayContractDetail> GetContractDetail(int contractId);
        Task<bool> UpdateContract(int contractId, Contract contract);
        Task<bool> DeleteContract(int contractId);
        Task<bool> UploadContractImg(int contractId, string customerImg, string contractImg);
        Task exporteExcel();
        Task<DisplayContractHomePage> getAllContractHomepage(int branchId);
        Task<bool> CreateContractExpiration(int contractId);
        Task<DisplayContractInfo> GetContractInfoByContractId(int contractId);
    }
}
