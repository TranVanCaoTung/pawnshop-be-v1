using Azure.Core;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        protected readonly DbContextClass _dbContext;
        public CustomerRepository(DbContextClass dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<Customer> getCustomerByCCCD(string cccd)
        {

            //var customer = from c in _dbContext.Customer.Where(c => c.CCCD == cccd)
            //               select c;
            var customer = _dbContext.Customer.SingleOrDefault(c => c.CCCD == cccd);

            return (Customer) customer;
        }
    }
}
