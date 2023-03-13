using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class Package
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int PackageInterest { get; set; }
        public int PaymentPeriod { get; set; }
        public int Day { get; set; }
        public int Limitation { get; set; }
        public int PunishDay1 { get; set; }
        public int PunishDay2 { get; set; }
        public int LiquitationDay { get; set; }

        public ICollection<Contract>? Contracts { get; set; }
        public Package()
        {
            Contracts = new List<Contract>();
        }

    }
}
