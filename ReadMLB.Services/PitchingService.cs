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
        Task BatchInsertPitchingStatAsync(ICollection<Pitching> currentBatch);
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

        public async Task BatchInsertPitchingStatAsync(ICollection<Pitching> currentBatch)
        {
            await _unitOfWork.PitchingStats.AddRangeAsync(currentBatch);
            await _unitOfWork.CompleteAsync();
        }
    }
}
