using System.Collections.Generic;
using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IDefenseStatsService
    {
        Task<long> AddDefenseStatsAsync(Defense newDefenseStat);
        Task CleanYearAsync(short year, bool inPO);
        Task<List<Defense>> GetPlayerDefenseStatsAsync(long playerId, bool inPo);
        Task BatchInsertDefenseStatAsync(IList<Defense> currentBatch);
    }
    public class DefenseStatsService : IDefenseStatsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DefenseStatsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<long> AddDefenseStatsAsync(Defense newDefenseStat)
        {
            var result = await _unitOfWork.DefenseStats.AddAsync(newDefenseStat);
            await _unitOfWork.CompleteAsync();
            return result.Entity.DefenseId;
        }

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Defense", year, inPO);
        }

        public Task<List<Defense>> GetPlayerDefenseStatsAsync(long playerId, bool inPo)
        {
            return _unitOfWork.DefenseStats.FindAsync(d => d.Team.Organization,
                d => d.PlayerId == playerId && d.InPO == inPo);
        }

        public async Task BatchInsertDefenseStatAsync(IList<Defense> defenseStats)
        {
            await _unitOfWork.DefenseStats.AddRangeAsync(defenseStats);
            await _unitOfWork.CompleteAsync();
        }
    }
}
