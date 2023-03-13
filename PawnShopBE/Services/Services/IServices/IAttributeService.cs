using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace Services.Services.IServices
{
    public interface IAttributeService
    {
        Task<bool> CreateAttribute(List<Attribute> attributes);
        Task<IEnumerable<Attribute>> GetAttributeByPawnableId(int pawnableProductId);
    }
}
