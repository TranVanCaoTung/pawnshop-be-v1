using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DependentService : IDependentService
    {
        private readonly IUnitOfWork _unit;
        public readonly DependentPeople dependentPeople;
        private readonly IServiceProvider _serviceProvider;
        public DependentService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            _unit = unitOfWork;
            _serviceProvider = serviceProvider;
        }
        public async Task<bool> CreateDependent(DependentPeople dependent)
        {
            if (dependent != null)
            {
                await _unit.DependentPeople.Add(dependent);
                var result = _unit.Save();
                if (result > 0)
                {
                    if (await plusPoint(dependent))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteDependent(Guid dependentId)
        {
            var dependentDelete = _unit.DependentPeople.SingleOrDefault(dependentPeople, j => j.DependentPeopleId == dependentId);
            if (dependentDelete != null)
            {
                _unit.DependentPeople.Delete(dependentDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<DependentPeople>> GetDependent()
        {
            var result = await _unit.DependentPeople.GetAll();
            return result;
        }

        public Task<DependentPeople> GetDependentById(Guid dependentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDependent(DependentPeople dependent)
        {
            var dependentUpdate = _unit.DependentPeople.SingleOrDefault(dependent, j => j.DependentPeopleId == dependent.DependentPeopleId);
            if (dependentUpdate != null)
            {
                dependentUpdate.DependentPeopleName = dependent.DependentPeopleName;
                dependentUpdate.CustomerRelationShip = dependent.CustomerRelationShip;
                dependentUpdate.MoneyProvided = dependent.MoneyProvided;
                dependentUpdate.Address = dependent.Address;
                dependentUpdate.PhoneNumber= dependent.Address;
                dependentUpdate.CustomerId = dependent.CustomerId;
                _unit.DependentPeople.Update(dependentUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> plusPoint(DependentPeople dependents)
        {
            var provider = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
            var customerList = await provider.GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == dependents.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 25;
            if (await provider.UpdateCustomer(customer))
                return true;
            else
                return false;
        }

    }
}
