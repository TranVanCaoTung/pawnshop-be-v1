using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
   public interface ICustomerService
    {
        Task<bool> CreateCustomer(Customer customer);
        Task<IEnumerable<Customer>> GetAllCustomer(int num);
        Task<Customer> GetCustomerById(Guid idCus);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(Guid customerId);
        Task<Customer> getCustomerByCCCD(string cccd);
        Task<CustomerDTO> getRelative(Guid idCus);
        Task<bool> createRelative(Guid idCus, CustomerDTO customer);
        Task<IEnumerable<DisplayCustomer>> getCustomerHaveBranch(
            IEnumerable<DisplayCustomer> respone, IEnumerable<Customer> listCustomer);
    }
}
