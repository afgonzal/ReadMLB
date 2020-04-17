using ReadMLB.DataLayer.Repositories;
using System.Threading.Tasks;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IPitchingService
    {
        Task CleanYearAsync(short year);
        Task<long> AddPitchingStatAsync(Pitching statsMajor);
    }
    public class PitchingService : IPitchingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PitchingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task CleanYearAsync(short year)
        {
            return _unitOfWork.CleanYearFromTableAsync("Pitching", year);
        }

        public async Task<long> AddPitchingStatAsync(Pitching pitchingStat)
        {
            var result = await _unitOfWork.PitchingStats.AddAsync(pitchingStat);
            await _unitOfWork.CompleteAsync();
            return result.Entity.PitchingId;
        }
    }
}
