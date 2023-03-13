using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IJobService
    {
        Task<bool> CreateJob(Job job);
        Task<IEnumerable<Job>> GetJob();
        Task<Job> GetJobById(int JobId);
        Task<bool> UpdateJob(Job job);
        Task<bool> DeleteJob(int JobId);
    }
}
