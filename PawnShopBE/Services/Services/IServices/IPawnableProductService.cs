using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IPawnableProductService
    {
        Task<bool> CreatePawnableProduct(PawnableProduct pawnableProduct);

        Task<IEnumerable<PawnableProduct>> GetAllPawnableProducts(int num);

        Task<PawnableProduct> GetPawnableProductById(int pawnableProductId);

        Task<bool> UpdatePawnableProduct(PawnableProduct pawnableProduct);
    }
}
