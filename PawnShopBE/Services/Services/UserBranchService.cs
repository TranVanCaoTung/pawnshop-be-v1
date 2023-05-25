using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserBranchService : IUserBranchService
    {
        public IUnitOfWork _unitOfWork;
        public DbContextClass _dbContextClass;
        public UserBranchService(IUnitOfWork unitOfWork, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
            _dbContextClass = dbContextClass;
        }
        public async Task<bool> CreateUserBranch(UserBranch userBranch)
        {
            await _unitOfWork.UserBranchs.Add(userBranch);
            var result = _unitOfWork.Save();

            return (result > 0) ? true : false;
        }

        public async Task<IEnumerable<UserBranch>> GetAllUserBranches()
        {
            var userBranchList = await _unitOfWork.UserBranchs.GetAll();
            return userBranchList;
        }

        public async Task<IEnumerable<UserBranch>> GetUserBranchByBranchId(int branchId)
        {
            var userBranchList = await  _dbContextClass.UserBranches.Where(x =>x.BranchId == branchId).ToListAsync();
            return userBranchList;
        }

        public async Task<UserBranch> GetUserBranchById(int id)
        {
            var userBranch = await _unitOfWork.UserBranchs.GetById(id);

            return (userBranch != null) ? userBranch : null;
        }

        public async Task<bool> UpdateUserBranch(UserBranch userBranch)
        {
            var userBranchUpdate = _dbContextClass.UserBranches.FirstOrDefault(x => x.UserId == userBranch.UserId);
            if (userBranchUpdate != null)
            {
                var existingBranchId = userBranchUpdate.BranchId;  // Store the existing BranchId

                _dbContextClass.UserBranches.Remove(userBranchUpdate);  // Delete the existing UserBranch entity
                _dbContextClass.SaveChanges();  // Save the deletion

                // Create a new UserBranch entity with the updated BranchId
                var newUserBranch = new UserBranch
                {
                    UserId = userBranch.UserId,
                    BranchId = userBranch.BranchId
                };

                _dbContextClass.UserBranches.Add(newUserBranch);  // Associate the new UserBranch entity with the new BranchId
                _dbContextClass.SaveChanges();  // Save the changes

                return true;
            }
            return false;
        }
    }
}
