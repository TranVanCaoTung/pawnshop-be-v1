using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AttributeService : IAttributeService
    {
        public IUnitOfWork _unitOfWork;

        public AttributeService(IUnitOfWork unitOfWork, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAttribute(List<PawnShopBE.Core.Models.Attribute> attributes)
        {
            if (attributes != null)
            {               
                await _unitOfWork.Attributes.AddList(attributes);               
                var result = await _unitOfWork.SaveList();

                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<PawnShopBE.Core.Models.Attribute>> GetAttributeByPawnableId(int pawnableProductId)
        {
            if (pawnableProductId != null)
            {
                var listPawnableProduct = await _unitOfWork.Attributes.GetAttributesByPawnableId(pawnableProductId);
  
                if (listPawnableProduct != null)
                {
                    return listPawnableProduct;
                }
            }
            return null;
        } 
    }
}    
