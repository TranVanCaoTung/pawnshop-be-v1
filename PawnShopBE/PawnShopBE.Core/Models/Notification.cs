using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int BranchId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
