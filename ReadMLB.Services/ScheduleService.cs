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
        Task AddScheduleAsync(IEnumerable<MatchResult> fullSchedule);

        Task CleanYearAsync(short year, bool inPO);
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

        public async Task AddScheduleAsync(IEnumerable<MatchResult> fullSchedule)
        {
            _unitOfWork.DisableTracking();
            await _unitOfWork.Schedule.AddRangeAsync(fullSchedule);
            await _unitOfWork.CompleteAsync();
            _unitOfWork.EnableTracking();
        }

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Matches", year, inPO);
        }
    }
}
