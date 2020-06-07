using System.Collections.Generic;
using ReadMLB.DataLayer.Repositories;
using System.Threading.Tasks;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IPitchingService
    {
        Task CleanYearAsync(short year, bool inPO);
        Task<long> AddPitchingStatAsync(Pitching statsMajor);
        Task<IEnumerable<Pitching>> GetPlayerPitchingHistoryAsync(long playerId);
        Task<IEnumerable<Pitching>> GetPlayerPitchingStatsAsync(long playerId, short year);
        Task<List<Pitching>> GetPlayerPitchingStatsAsync(long playerId, bool inPO = false);
        Task BatchInsertPitchingStatAsync(ICollection<Pitching> currentBatch);
        Task<IEnumerable<Pitching>> GetLeaguePitchingStatsLeadersAsync(byte league, short year, bool inPo,byte? teamId, int take);
    }
    public class PitchingService : IPitchingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PitchingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Pitching", year, inPO);
        }

        public async Task<long> AddPitchingStatAsync(Pitching pitchingStat)
        {
            var result = await _unitOfWork.PitchingStats.AddAsync(pitchingStat);
            await _unitOfWork.CompleteAsync();
            return result.Entity.PitchingId;
        }

        public Task<IEnumerable<Pitching>> GetPlayerPitchingHistoryAsync(long playerId)
        {
            return _unitOfWork.PitchingStats.FindAsync(ps => ps.PlayerId == playerId);
        }

        public Task<IEnumerable<Pitching>> GetPlayerPitchingStatsAsync(long playerId, short year)
        {
            return _unitOfWork.PitchingStats.FindAsync(ps => ps.PlayerId == playerId && ps.Year == year);
        }

        public Task<List<Pitching>> GetPlayerPitchingStatsAsync(long playerId, bool inPO = false)
        {
            return _unitOfWork.PitchingStats.FindAsync(p => p.Team.Organization,
                p => p.PlayerId == playerId && p.InPO == inPO);
        }

        public async Task BatchInsertPitchingStatAsync(ICollection<Pitching> currentBatch)
        {
            await _unitOfWork.PitchingStats.AddRangeAsync(currentBatch);
            await _unitOfWork.CompleteAsync();
        }

        public Task<IEnumerable<Pitching>> GetLeaguePitchingStatsLeadersAsync(byte league, short year, bool inPo,
            byte? teamId, int take)
        {
            if (!teamId.HasValue)
                return _unitOfWork.PitchingStats.FindAsync(new List<string> { "Team", "Player"},
                    p => p.League == league && p.Year == year && p.InPO == inPo, p => p.IP10,true, take, 0);
            return _unitOfWork.PitchingStats.FindAsync(new List<string> { "Team", "Player" },
                p => p.League == league && p.Year == year && p.InPO == inPo && (p.TeamId == teamId.Value || p.Team.OrganizationId == teamId.Value), p => p.IP10, true, take, 0);

        }
    }
}
