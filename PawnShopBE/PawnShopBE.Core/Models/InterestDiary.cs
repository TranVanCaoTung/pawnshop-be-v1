using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class InterestDiary
    {

        public int? InterestDiaryId { get; set; }
        public int ContractId { get; set; }
        public decimal Payment { get; set; }
        public decimal Penalty { get; set; }
        public decimal TotalPay { get; set; }
        public decimal PaidMoney { get; set; }
        public decimal InterestDebt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime NextDueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }

        public virtual Contract Contract { get; set; }
        public ICollection<DiaryImg>? DiaryImgs { get; set; }

        public InterestDiary()
        {
            DiaryImgs = new List<DiaryImg>();
        }
    }
}
