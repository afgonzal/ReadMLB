using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadMLB.Services
{
    public interface IPlayersService
    {
        Task<int> AddAsync(Player newPlayer);

        ValueTask<Player> GetByIdAsync(long id);

        Task<int> UpdateAsync(Player player);
        Task<IEnumerable<Player>> GetAll();

        Task<Player> FindEAPlayerAsync(long eaId, short year);
    }
    public class PlayersService : IPlayersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddAsync(Player newPlayer)
        {
                await _unitOfWork.Players.AddAsync(newPlayer);
                return await _unitOfWork.CompleteAsync();
        }

        public async ValueTask<Player> GetByIdAsync(long id)
        {
            return await _unitOfWork.Players.GetAsync(id);
        }

        public async Task<int> UpdateAsync(Player player)
        {
            await _unitOfWork.Players.UpdateAsync(player);
            return await _unitOfWork.CompleteAsync();
        }

        public Task<IEnumerable<Player>> GetAll()
        {
            return _unitOfWork.Players.FindAsync(p => !p.IsInvalid);
        }

        public Task<Player> FindEAPlayerAsync(long eaId, short year)
        {
            return _unitOfWork.Players.SingleOrDefaultAsync(p => p.EAId == eaId && p.Year == year);
        }
    }
}
