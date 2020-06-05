using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadMLB.Services
{
    public interface IPlayersService
    {
        Task<int> AddAsync(Player newPlayer);

        ValueTask<Player> GetByIdAsync(long id, bool inPO);

        Task<int> UpdateAsync(Player player);
        Task<IEnumerable<Player>> GetAll();

        Task<Player> FindEAPlayerAsync(long eaId, short year, string fName, string lName);
        Task<int> UpdatePlayerAttributesAsync(Player player);
        Task CleanYearAsync(short year);

        Task<IEnumerable<Player>> SearchAsync(byte? league, short? year, string firstName, string lastName,
            PlayerPositionAbr? position);
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

        public async ValueTask<Player> GetByIdAsync(long id, bool inPO = false)
        {
            var player = await _unitOfWork.Players.SingleOrDefaultAsync(p => p.PlayerId == id);
            if (player == null)
                return null;
            player.RosterHistory = await this.PlayerRosterHistoryAsync(id, false);
            if (player.RosterHistory.Any())
                player.Team = player.RosterHistory.First().Team;
            return player;
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

        public async Task<Player> FindEAPlayerAsync(long eaId, short year, string fName, string lName)
        {
            var players = (await _unitOfWork.Players.FindAsync(p => p.EAId == eaId && p.Year <= year)).ToList();
            if (!players.Any())
                return null;
            if (players.Count() == 1)
                return players.Single();

            return players.Where(p => p.FirstName == fName && p.LastName == lName).OrderByDescending(p => p.Year)
                .FirstOrDefault();
        }

        public async Task<int> UpdatePlayerAttributesAsync(Player player)
        {
            await _unitOfWork.Players.UpdateAsync(player, new string[] {"Shirt", "PrimaryPosition", "SecondaryPosition", "Bats", "Throws"});
            return await _unitOfWork.CompleteAsync();
        }

        public Task CleanYearAsync(short year)
        {
            return _unitOfWork.CleanYearFromTableAsync("Players", year);
        }

      

        public Task<IEnumerable<Player>> SearchAsync(byte? league, short? year, string firstName, string lastName,
            PlayerPositionAbr? position)
        {
            return _unitOfWork.Players.SearchPlayers(league, year, firstName, lastName, position);
        }

        public async Task<IEnumerable<RosterPosition>> PlayerRosterHistoryAsync(long playerId, bool inPO = false)
        {
            return await _unitOfWork.Rosters.FindAsync(rp => rp.Team.Organization, rp => rp.PlayerId == playerId && rp.InPO == inPO,
                rp => rp.Year);
        }
    }
}
