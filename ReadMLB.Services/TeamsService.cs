using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadMLB.Services
{
    public interface ITeamsService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        ValueTask<Team> GetTeamByIdAsync(byte id);
    }

    public class TeamsService : ITeamsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            var teams = await _unitOfWork.Teams.GetAllAsync();
            return teams.OrderBy(t => t.TeamId);
        }

        public ValueTask<Team> GetTeamByIdAsync(byte id)
        {
            return _unitOfWork.Teams.GetAsync(id);
        }
    }
}
