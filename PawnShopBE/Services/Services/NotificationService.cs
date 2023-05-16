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
        private DbContextClass _dbContextClass;
        private IUnitOfWork _unitOfWork;

        public NotificationService(DbContextClass dbContextClass, IUnitOfWork unitOfWork)
        {           
            _dbContextClass = dbContextClass;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> UpdateNotification(int notificationId, bool isRead)
        {
            var notification = _dbContextClass.Notifications.FirstOrDefault(x => x.NotificationId == notificationId);
            if (notification != null)
            {
                notification.IsRead = isRead;
                _unitOfWork.Notifications.Update(notification);
                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
