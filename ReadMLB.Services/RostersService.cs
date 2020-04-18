using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IRostersService
    {
        Task AddRosterAsync(RosterPosition newRosterPosition);
        Task CleanYearAsync(short year);
        Task<RosterPosition> FindByPlayerAsync(long playerId);
    }
    public class RostersService : IRostersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RostersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRosterAsync(RosterPosition newRosterPosition)
        {
            await _unitOfWork.Rosters.AddAsync(newRosterPosition);
            await _unitOfWork.CompleteAsync();
        }

        public Task CleanYearAsync(short year)
        {
            return _unitOfWork.CleanYearFromTableAsync("Rosters", year);
        }

        public Task<RosterPosition> FindByPlayerAsync(long playerId)
        {
            return _unitOfWork.Rosters.SingleOrDefaultAsync(r => r.PlayerId == playerId);
        }
    }
}
