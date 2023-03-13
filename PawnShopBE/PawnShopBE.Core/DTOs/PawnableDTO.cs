using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class PawnableDTO
    {
        public string TypeOfProduct { get; set; }
        public string CommodityCode { get; set; }
        public int Status { get; set; }
        public ICollection<AttributeDTO>? AttributeDTOs { get; set; }
    }
}
