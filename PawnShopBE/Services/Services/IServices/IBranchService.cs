using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IBranchService
    {
        Task<bool> CreateBranch(Branch branch);

        Task<IEnumerable<Branch>> GetAllBranch(int num);

        Task<Branch> GetBranchById(int branchId);

        Task<bool> UpdateBranch(int id, BranchRequest branch);

        Task<bool> DeleteBranch(int branchId);
        Task<IEnumerable<DisplayBranch>> getDisplayBranch();
        Task<DisplayBranchDetail> getDisplayBranchDetail(int branchId);
        Task<DisplayBranchDetail> getDisplayBranchYearDetail(int branchId, int year);

    }
}
