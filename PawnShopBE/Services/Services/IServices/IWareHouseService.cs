using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IWareHouseService
    {
        Task<bool> CreateWareHouse(Warehouse warehouse);
        Task<IEnumerable<Warehouse>> GetWareHouse(int num);
        Task<Warehouse> GetWareHouseById(int wareHouseId);
        Task<bool> UpdateWareHouse(Warehouse warehouse);
        Task<bool> DeleteWareHouse(int wareHouseId);
        Task<WareHouseDTO> getWareHouseDetail(int id,int num);
    }
}
