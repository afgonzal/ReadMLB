
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReadMLB.Services
{
    public interface IBattingService
    {
        Task<long> AddBattingStatAsync(Batting battingStat);

        Task TruncateBattingStatsAsync();
    }
    public class BattingService : IBattingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BattingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<long> AddBattingStatAsync(Batting battingStat)
        {
            try
            {
                var result = await _unitOfWork.BattingStats.AddAsync(battingStat);
                await _unitOfWork.CompleteAsync();
                return result.Entity.BattingId;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        public Task TruncateBattingStatsAsync()
        {
            return _unitOfWork.TruncateTableAsync("Batting");
        }
    }
}
