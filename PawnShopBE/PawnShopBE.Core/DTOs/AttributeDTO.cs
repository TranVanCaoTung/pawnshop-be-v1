using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class AttributeDTO
    {
        public int? PawnableProductId { get; set; }
        public string Description { get; set; }
    }
}
