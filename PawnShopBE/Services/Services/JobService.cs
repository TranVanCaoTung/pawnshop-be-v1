using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unit;
        private readonly Job job;
        private readonly IServiceProvider _serviceProvider;

        public JobService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) { 
              _unit=unitOfWork;
            _serviceProvider=serviceProvider;
        }
        public async Task<bool> CreateJob(Job job)
        {
            if (job != null)
            {
                await _unit.Jobs.Add(job);
                var result = _unit.Save();
                if (result > 0)
                {
                    if(await plusPoint(job))
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> plusPoint(Job job)
        {
            var provider = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
            var customerList = await provider.GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == job.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 50;
            if( await provider.UpdateCustomer(customer))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public async Task<bool> DeleteJob(int JobId)
        {
           var jobDelete=  _unit.Jobs.SingleOrDefault(job,j=> j.JobId == JobId);
            if (jobDelete != null)
            {
                _unit.Jobs.Delete(jobDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Job>> GetJob()
        {
            var result=await _unit.Jobs.GetAll();
            return result;
        }

        public Task<Job> GetJobById(int JobId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateJob(Job job)
        {
            var jobUpdate = _unit.Jobs.SingleOrDefault(job, j => j.JobId == job.JobId);
            if(jobUpdate!= null)
            {
                jobUpdate.NameJob = job.NameJob;
                jobUpdate.Salary=job.Salary;
                jobUpdate.WorkLocation=job.WorkLocation;
                jobUpdate.IsWork=job.IsWork;
                jobUpdate.LaborContract=job.LaborContract;
                jobUpdate.Customer=job.Customer;
                _unit.Jobs.Update(jobUpdate);
                var result= _unit.Save();
                if(result> 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
