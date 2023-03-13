using Azure;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork _unitOfWork;
        public IKycService _kycService;
        public ICustomerRepository _customerRepository;
        private readonly IContractService _contract;
        private readonly IBranchService _branch;
        private readonly IJobService _job;
        private readonly ICustomerRelativeService _relative;
        private readonly IDependentService _dependent;

        public CustomerService(IUnitOfWork unitOfWork, IKycService kycService, ICustomerRepository customerRepository, IContractService contract,
           IBranchService branch, IJobService job, ICustomerRelativeService relativeService,
           IDependentService dependent)
        {
            _unitOfWork = unitOfWork;
            _kycService= kycService;
            _customerRepository= customerRepository;
            _branch = branch;
            _contract = contract;
            _job = job;
            _relative = relativeService;
            _dependent = dependent;
        }

        public async Task<CustomerDTO> getRelative(Guid idCus)
        {
            var listCustomer = await GetAllCustomer(0);
            //get Job
            var listJob =await _job.GetJob();
            var Job = from b in listJob where b.CustomerId==idCus select b;

            //get Relative
            var listRelative = await _relative.GetCustomerRelative();
            var Relative=from r in listRelative where r.CustomerId==idCus select r;

            //get Dependent
            var listDependent = await _dependent.GetDependent();
            var dependent= from d in listDependent where d.CustomerId==idCus select d;

            //trien khai CustomerDTO
            var customerDTOs = new CustomerDTO();
            try
            {
                customerDTOs.Jobs= new List<JobDTO>();
                customerDTOs.DependentPeople= new List<DependentPeopleDTO>();
                customerDTOs.CustomerRelativeRelationships = new List<CustomerRelativeDTO>();
                //đưa vào list trong customer
                customerDTOs = getListRelativeDTO(idCus,customerDTOs,Job,Relative,dependent);
                return customerDTOs;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private CustomerDTO getListRelativeDTO(Guid idCus,CustomerDTO customerDTOs, IEnumerable<Job> job, 
            IEnumerable<CustomerRelativeRelationship> relative, IEnumerable<DependentPeople> dependent)
        {
            foreach (var y in job)
            {
                var x = new JobDTO();
                x.CustomerId = idCus;
                x.NameJob = y.NameJob;
                x.IsWork = y.IsWork;
                x.WorkLocation = y.WorkLocation;
                x.LaborContract = y.LaborContract;
                x.Salary = y.Salary;
                customerDTOs.Jobs.Add(x);
            }
            foreach (var x in dependent)
            {
                var y = new DependentPeopleDTO();
                y.CustomerId = idCus;
                y.DependentPeopleName = x.DependentPeopleName;
                y.CustomerRelationShip = x.CustomerRelationShip;
                y.MoneyProvided = x.MoneyProvided;
                y.Address = x.Address;
                y.PhoneNumber = x.PhoneNumber;
                customerDTOs.DependentPeople.Add(y);
            }
            foreach (var x in relative)
            {
                var y = new CustomerRelativeDTO();
                y.CustomerId = idCus;
                y.RelativeName = x.RelativeName;
                y.RelativeRelationship = x.RelativeRelationship;
                y.Salary = x.Salary;
                y.Address = x.Address;
                y.RelativePhone = x.RelativePhone;
                customerDTOs.CustomerRelativeRelationships.Add(y);
            }
            return customerDTOs;
        }

        public async Task<bool> createRelative(Guid idCus,CustomerDTO customer)
        {
            var job = new Job();
            var relative = new CustomerRelativeRelationship();
            var dependent = new DependentPeople();

            //get id Cus for All Relative
            job.CustomerId = idCus;
            relative.CustomerId = idCus;
            dependent.CustomerId = idCus;

            //create relative
            try
            {
                //get field for relative
                job = getFieldJob(customer,job);
                relative=getFieldRelative(customer,relative);
                dependent=getFieldDependent(customer,dependent);
               
                //create relative
                var createRelative = _relative.CreateCustomerRelative(relative);
                var createJob = _job.CreateJob(job);
                var createDependent = _dependent.CreateDependent(dependent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private DependentPeople getFieldDependent(CustomerDTO customer, DependentPeople dependent)
        {
            foreach (var x in customer.DependentPeople)
            {
                dependent.DependentPeopleName = x.DependentPeopleName;
                dependent.CustomerRelationShip = x.CustomerRelationShip;
                dependent.MoneyProvided = x.MoneyProvided;
                dependent.Address = x.Address;
                dependent.PhoneNumber = x.PhoneNumber;
            }
            return dependent;
        }

        private CustomerRelativeRelationship getFieldRelative(CustomerDTO customer, CustomerRelativeRelationship relative)
        {
            foreach (var x in customer.CustomerRelativeRelationships)
            {
                relative.RelativeName = x.RelativeName;
                relative.RelativeRelationship = x.RelativeRelationship;
                relative.Salary = x.Salary;
                relative.Address = x.Address;
                relative.RelativePhone = x.RelativePhone;
            }
            return relative;
        }

        private Job getFieldJob(CustomerDTO customer, Job job)
        {
            foreach (var x in customer.Jobs)
            {
                job.WorkLocation = x.WorkLocation;
                job.NameJob = x.NameJob;
                job.Salary = x.Salary;
                job.LaborContract = x.LaborContract;
                job.IsWork = x.IsWork;
            }
            return job;
        }

        public async Task<bool> CreateCustomer(Customer customer)
        {
            var oldCus = GetCustomerById(customer.CustomerId);
            // Check if new customer
            if (oldCus == null)
            {       
                customer.Kyc = null;
                customer.DependentPeople = null;
                customer.Jobs = null;
                customer.CustomerRelativeRelationships = null;
                customer.Status = (int)CustomerConst.ACTIVE;
                customer.Point = 0;
                customer.CreatedDate = DateTime.Now;
                await _unitOfWork.Customers.Add(customer);
            }
            return false;
           
        }
        public async Task<IEnumerable<Customer>> GetAllCustomer(int num)
        {
            var listCustomer = await _unitOfWork.Customers.GetAll();
            if (num == 0)
            {
                return listCustomer;
            }
            var result = await _unitOfWork.Customers.TakePage(num, listCustomer);
            return result;
        }

        public async Task<Customer> getCustomerByCCCD(string cccd)
        {
            if (cccd != null)
            {
                var customer = await _customerRepository.getCustomerByCCCD(cccd);
                if (customer != null)
                {
                    return customer;
                }
            }
            return null;
        }

        public async Task<IEnumerable<DisplayCustomer>> getCustomerHaveBranch(
            IEnumerable<DisplayCustomer> respone, IEnumerable<Customer> listCustomer)
        {
            int i = 1;
            foreach (var customer in respone)
            {
                // lấy customer id
                var customerId = customer.customerId;
                // lấy branchName
                customer.nameBranch = await GetBranchName(customerId, listCustomer);
                customer.numerical = i++;
            }
            return respone;
        }
        private async Task<string> GetBranchName(Guid customerId, IEnumerable<Customer> listCustomer)
        {
            //lấy danh sách contract
            var listContract = await _contract.GetAllContracts(0);
            // lấy branch id mà customer đang ở
            var branch = listCustomer.Join(listContract, p => p.CustomerId, c => c.CustomerId
                    , (p, c) => { return c.BranchId; });
            var branchtId = branch.First();
            // lấy danh sách branch
            var listBranch = await _branch.GetAllBranch(0);
            // lấy branchname
            var branchName = listContract.Join(listBranch, c => c.BranchId, b => b.BranchId,
                (c, b) => { return b.BranchName; });
            var name = branchName.First().ToString();
            return name;
        }
        public async Task<Customer> GetCustomerById(Guid idCus)
        {
            if (idCus != null)
            {
                var customer =await _unitOfWork.Customers.GetById(idCus);
                if(customer != null)
                {
                    return customer;
                }
            }
            return null;
        }
        public async Task<bool> DeleteCustomer(Guid customerId)
        {
            if (customerId != null)
            {
                var customer = await _unitOfWork.Customers.GetById(customerId);
                if (customer != null)
                {
                    _unitOfWork.Customers.Delete(customer);
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
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (customer != null)
            {
                var customerUpdate = _unitOfWork.Customers.SingleOrDefault(customer, en => en.CustomerId ==customer.CustomerId);
                if (customerUpdate != null)
                {
                    // customerUpdate = customer;
                    customerUpdate.Status = customer.Status;
                    customerUpdate.Point = customer.Point;
                    customerUpdate.CCCD = customer.CCCD;
                    customerUpdate.Phone = customer.Phone;
                    customerUpdate.Address = customer.Address;
                    customerUpdate.FullName = customer.FullName;
                    customerUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Customers.Update(customerUpdate);
                    var result = _unitOfWork.Save();
                    if(result > 0)
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
