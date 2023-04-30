using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IInteresDiaryService _interesDiaryService;
        private readonly DbContextClass _dbContextClass;
        public NotificationService(IInteresDiaryService interesDiaryService, DbContextClass dbContextClass)
        {
            _interesDiaryService = interesDiaryService;
            _dbContextClass = dbContextClass;
        }

        public async Task<bool> CreateNotification(int branchId)
        {
        //    _dbContextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary ON;");
        //    interestDiaries.Add(interestDiary);
        //    await _unit.InterestDiaries.AddList(interestDiaries);
        //    _dbContextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary OFF;");

        //}
        //result = await _unit.SaveList();

        //        if (result > 0)
        //        {
        //            return true;
        //        }
            return false;
        }

        public async Task<IEnumerable<DisplayNotification>> NotificationList(int branchId)
        {
            var notifiList = new List<DisplayNotification>();
            return notifiList;
        }
    }
}
