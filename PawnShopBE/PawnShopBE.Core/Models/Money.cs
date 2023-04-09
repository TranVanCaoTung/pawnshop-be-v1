using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    [Table("Money")]
    public class Money
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MoneyId { get; set; }
        public decimal? Fund { get; set; }
        public int BranchId { get; set; }
        public DateTime DateCreate { get; set; }
        public string UserName { get; set; }
        public decimal MoneyInput { get; set; }
        public int Status { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

    }
}
