using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class LogContract
    {
        public int LogContractId { get; set; }
        public int ContractId { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public int EventType { get; set; }
        public DateTime LogTime { get; set; }
        public decimal Debt { get; set; }
        public decimal Paid { get; set; }
        public string? Description { get; set; }

        public virtual Contract Contract { get; set; }

    }
}
