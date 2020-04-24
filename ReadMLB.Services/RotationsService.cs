using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IRotationsService
    {
        Task AddRotationPositionAsync(RotationPosition newRotationPosition);
        Task CleanYearAsync(short year, bool inPO);
    }
    public class RotationsService : IRotationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RotationsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddRotationPositionAsync(RotationPosition newRotationPosition)
        {
            var result = await _unitOfWork.Rotations.AddAsync(newRotationPosition);
            await _unitOfWork.CompleteAsync();
        }

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Rotations", year, inPO);
        }
    }
}
