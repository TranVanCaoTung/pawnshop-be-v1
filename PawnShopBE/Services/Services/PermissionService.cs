using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PawnShopBE.Core.Display;
using static Quartz.Logging.OperationName;
using System.Security.Cryptography;

namespace Services.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly IUnitOfWork _unit;
        
        private readonly Permission _permission;

        public PermissionService(IUnitOfWork unitOfWork) { 
              _unit=unitOfWork;
        }
        public async Task SavePermission(IEnumerable<DisplayPermission> listPermission)
        {
            var listGroup = await _unit.UserPermissionGroup.GetAll();
            foreach(var p in listPermission)
            {
                UserPermission group= new UserPermission();
                group.UserId= p.UserId;
                group.PerId = p.PermissionId;
                //check field đã tồn tại hay chưa
                var result = _unit.UserPermissionGroup.
                    SingleOrDefault(group,g => g.UserId==group.UserId && g.PerId ==group.PerId);
                if (result == null)
                {
                    await _unit.UserPermissionGroup.Add(group);
                }
                else
                {
                     result.Status = p.Status;
                     _unit.UserPermissionGroup.Update(result);
                }
                 _unit.Save();
            }
        }

        public async Task<IEnumerable<DisplayPermission>> ShowPermission(Guid userId)
        {
            //get list all
            var listPermission = await GetPermission();
            var listGroup = await _unit.UserPermissionGroup.GetAll();

            List<DisplayPermission> list = new List<DisplayPermission>();
            foreach(var p in listPermission)
            {
                DisplayPermission permission= new DisplayPermission();
                permission.PermissionId = p.PerId;
                permission.UserId = userId;
                permission.NamePermission = p.Description;
                permission.Status = getStatus(listGroup, p.PerId, userId);
                list.Add(permission);
            }
            return list;
        }

        private bool getStatus(IEnumerable<UserPermission> listGroup, int perId, Guid userId)
        {
            var result = (from p in listGroup where p.PerId == perId && p.UserId == userId select p).FirstOrDefault();
            if (result!=null)
            {
                return result.Status;
            }
            return false;
        }

        public async Task<bool> CreatePermission(Permission permission)
        {
            var listPermisstion = await GetPermission();
            var checkPermission= from p in listPermisstion where p.Description.CompareTo(permission.Description) == 0 select p;

            if (checkPermission.Count()<1)
            {
                await _unit.Permission.Add(permission);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<bool> DeletePermission(int perId)
        {
            var perDelete = _unit.Permission.SingleOrDefault(_permission, j => j.PerId==perId );
            if (perDelete != null)
            {
                _unit.Permission.Delete(perDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
       

        public async Task<IEnumerable<Permission>> GetPermission()
        {
            var result = await _unit.Permission.GetAll();
            
            return result;
        }

        public async Task<bool> UpdatePermission(Permission permission)
        {
            var perUpdate = _unit.Permission.SingleOrDefault(permission, j => j.PerId == permission.PerId);
            if (perUpdate != null)
            {
                perUpdate.NamePermission= permission.NamePermission;
                perUpdate.Description = permission.Description;
                _unit.Permission.Update(perUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
