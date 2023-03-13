using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Interfaces
{
    public interface IAttributeRepository : IGenericRepository<Models.Attribute>
    {
        public Task<List<Models.Attribute>> GetAttributesByPawnableId(int pawmableProductId);
    }
}
