using Microsoft.EntityFrameworkCore;
using Mysqlx.Resultset;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Helpers;
using PawnShopBE.Infrastructure.Helpers;
using Quartz;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MonthlyJob : IJob
    {

        private readonly DbContextClass _contextClass;
        private readonly IContractService _contractService;
        private readonly IInteresDiaryService _interesDiaryService;
        private readonly ILedgerService _ledgerService;
        private readonly IBranchService _branchService;
        private readonly IRansomService _ransomService;
        private readonly ILiquidationService _liquidationService;
        private readonly IUnitOfWork _unitOfWork;

        public MonthlyJob(DbContextClass dbContextClass, IContractService contractService, IRansomService ransomService, IInteresDiaryService interesDiaryService, ILogContractService logContractService, ILedgerService ledgerService, IBranchService branchService, ILiquidationService liquidationService, IUnitOfWork unitOfWork)
        {
            _contextClass = dbContextClass;
            _contractService = contractService;
            _ransomService = ransomService;
            _interesDiaryService = interesDiaryService;
            _ledgerService = ledgerService;
            _branchService = branchService;
            _liquidationService = liquidationService;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Create notification for contract end date.
            var contractsListToday = await _contextClass.Contract.Where(x => x.ContractEndDate.Date == DateTime.Now.Date).ToListAsync();
            // Get notfication have been created today
            var notificationListToday = await _contextClass.Notifications.Where(x => x.CreatedDate.Date == DateTime.Now.Date).ToListAsync();
            if (notificationListToday.Count != contractsListToday.Count)
            {
                var notificationList = new List<Notification>();
                foreach (var contract in contractsListToday)
                {
                    // Create new notification list when list is zero
                    if (notificationListToday.Count == 0)
                    {
                        // Create new instance to save list of notification
                        var notifi = new Notification();
                        notifi.BranchId = contract.BranchId;
                        notifi.Header = "Hợp đồng đến hạn";
                        notifi.Content = "Hợp đồng " + contract.ContractCode + " đã đến hạn cần thanh toán.";
                        notifi.Type = (int)NotificationConst.CONTRACT_END_DATE;
                        notifi.CreatedDate = DateTime.Now;
                        notifi.IsRead = false;
                        notificationList.Add(notifi);
                    }
                    //else
                    //{
                    //    foreach (var notification in notificationListToday)
                    //    {
                    //        // Matches "CĐ-" followed by one or more digits
                    //        string pattern = @"CĐ-\d+";
                    //        Match match = Regex.Match(notification.Content, pattern);
                    //        if (match.Success)
                    //        {
                    //            // Check if notification is created
                    //            if (contract.ContractCode.Equals(match.Value))
                    //            {
                    //                continue;
                    //            }
                    //            else
                    //            {
                    //                // Create new instance to save list of notification
                    //                var notifi = new Notification();
                    //                notifi.BranchId = contract.BranchId;
                    //                notifi.Header = "Hợp đồng đến hạn";
                    //                notifi.Content = "Hợp đồng " + contract.ContractCode + " đã đến hạn cần thanh toán.";
                    //                notifi.Type = (int)NotificationConst.CONTRACT_END_DATE;
                    //                notifi.CreatedDate = DateTime.Now;
                    //                notifi.IsRead = false;
                    //                notificationList.Add(notifi);

                    //            }
                    //        }
                    //    }

                    //}

                }
                _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Notification ON;");
                await _unitOfWork.Notifications.AddList(notificationList);
                _contextClass.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.InterestDiary OFF;");
            }
            var userBranchList = _unitOfWork.UserBranchs.GetAll();

            string _gmail = "hethongpawns@gmail.com";
            string _password = "fnblmxkfeaeilbxs";

            //string sendto = user.Email;
            string subject = "[PAWNSHOP] - HỢP ĐỒNG CẦN XỬ LÝ";
            string content = DateTime.Now.ToString("dd/MM/yyyy") + "Hiện tại đang có " + "14" + " hợp đồng cần được xử lí ";

   
            var result = _unitOfWork.Save();

            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //set property for email you want to send
            mail.From = new MailAddress(_gmail);
            //mail.To.Add(sendto);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;
            mail.Priority = MailPriority.High;
            //set smtp port
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            //set gmail pass sender
            smtp.Credentials = new NetworkCredential(_gmail, _password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
            _contextClass.SaveChanges();
        }
    }
}
