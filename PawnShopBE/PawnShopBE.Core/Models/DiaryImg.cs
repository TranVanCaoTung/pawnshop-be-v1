using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class DiaryImg
    {
        public int DiaryImgId { get; set; }
        public int InterestDiaryId { get; set; }
        public string ProofImg { get; set; }
        public DateTime PaidDate { get; set; } 
        public virtual InterestDiary InterestDiary {get;set; }
    }
}
