using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Interfaces;
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

        public Task<IEnumerable<DisplayNotification>> NotificationList(int branchId)
        {
            throw new NotImplementedException();
        }
    }
}
