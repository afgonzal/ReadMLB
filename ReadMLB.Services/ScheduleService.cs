using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IScheduleService
    {
        Task<long> AddMatchAsync(MatchResult newMatch);
    }
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddMatchAsync(MatchResult newMatch)
        {
            var result = await _unitOfWork.Schedule.AddAsync(newMatch);
            await _unitOfWork.CompleteAsync();
            return result.Entity.MatchId;
        }
    }
}
