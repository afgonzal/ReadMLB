using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IRunningService
    {
        Task<long> AddRunningStat(Running newRunningStat);
    }
    public class RunningService : IRunningService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RunningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddRunningStat(Running newRunningStat)
        {
            var result = await _unitOfWork.RunningStats.AddAsync(newRunningStat);
            await _unitOfWork.CompleteAsync();
            return result.Entity.RunningId;
        }
    }
}
