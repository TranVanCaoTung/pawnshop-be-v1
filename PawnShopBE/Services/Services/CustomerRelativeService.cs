using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CustomerRelativeService : ICustomerRelativeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerRelativeRelationship customerRelativeRelationship;
        public CustomerRelativeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateCustomerRelative(CustomerRelativeRelationship customerRelative)
        {
            if (customerRelative != null)
            {
                await _unitOfWork.CustomersRelativeRelationships.Add(customerRelative);
                var result = _unitOfWork.Save();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteCustomerRelative(Guid CustomerRelativeId)
        {
            if (CustomerRelativeId != null)
            {
                var customer = _unitOfWork.CustomersRelativeRelationships.
                    SingleOrDefault(customerRelativeRelationship, j => j.CustomerRelativeRelationshipId == CustomerRelativeId);
                if (customer != null)
                {
                    _unitOfWork.CustomersRelativeRelationships.Delete(customer);
                    var result = _unitOfWork.Save();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public async Task<IEnumerable<CustomerRelativeRelationship>> GetCustomerRelative()
        {
            var listCustomer = await _unitOfWork.CustomersRelativeRelationships.GetAll();
             return listCustomer;
        }

        public Task<Job> GetCustomerRelativeById(Guid CustomerRelativeId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCustomerRelative(CustomerRelativeRelationship customerRelative)
        {
            if (customerRelative != null)
            {
                var customerUpdate = _unitOfWork.CustomersRelativeRelationships
                    .SingleOrDefault(customerRelative, en => en.CustomerRelativeRelationshipId
                    == customerRelative.CustomerRelativeRelationshipId);
                if (customerUpdate != null)
                {
                    // customerUpdate = customer;
                    customerUpdate.RelativeRelationship = customerRelative.RelativeRelationship;
                    customerUpdate.Salary = customerRelative.Salary;
                    customerUpdate.Address= customerRelative.Address;
                    customerUpdate.RelativePhone = customerRelative.RelativePhone;
                    customerUpdate.CustomerId = customerRelative.CustomerId;
                    customerUpdate.RelativeName = customerRelative.RelativeName;
                    _unitOfWork.CustomersRelativeRelationships.Update(customerUpdate);
                    var result = _unitOfWork.Save();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
