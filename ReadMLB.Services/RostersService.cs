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
        Task CleanYearAsync(short year, bool inPO);
        Task<RosterPosition> FindByPlayerAsync(long playerId, short year, bool inPO);

        Task<IEnumerable<RosterPosition>> GetTeamRosterAsync(byte teamId, short year, bool inPO);

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

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Rosters", year, inPO);
        }

        public Task<RosterPosition> FindByPlayerAsync(long playerId, short year, bool inPO)
        {
            return _unitOfWork.Rosters.SingleOrDefaultAsync(r => r.PlayerId == playerId && r.Year == year && r.InPO == inPO);
        }

        public Task<IEnumerable<RosterPosition>> GetTeamRosterAsync(byte teamId, short year, bool inPO)
        {
            return _unitOfWork.Rosters.FindAsync(r => r.Player,
                r => r.TeamId == teamId && r.Year == year && r.InPO == inPO, r => r.Slot);
        }
    }
}
