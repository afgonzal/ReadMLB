using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IDefenseStatsService
    {
        Task<long> AddDefenseStatsAsync(Defense newDefenseStat);
        Task CleanYearAsync(short year, bool inPO);
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
    }
}
