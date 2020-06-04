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

        Task<Team> GetTeamByIdAsync(byte teamId);

        Task<IEnumerable<Team>> GetTeamOrganizationAsync(byte teamId);

        Task<IEnumerable<Team>> GetOrganizationById(byte organizationId);
        Task<List<Team>> GetLeagueTeamsAsync(byte league);
        Task<IEnumerable<Division>> GetLeagueDivisionsAsync(byte league);
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
            var teams = await _unitOfWork.Teams.FindAsync(t  => t.Organization, t=>t.League.HasValue);
            return teams.OrderBy(t => t.TeamId);
        }

        public async Task<IEnumerable<Team>> GetTeamOrganizationAsync(byte teamId)
        {
            var team = await GetTeamByIdAsync(teamId);
            if (team.OrganizationId.HasValue)
            {
                return await _unitOfWork.Teams.FindAsync(t => t.Organization, t =>
                    t.OrganizationId == team.OrganizationId.Value || t.TeamId == team.OrganizationId, t => t.League);
            }
            else
            {
                return await _unitOfWork.Teams.FindAsync(t => t.Organization, t =>
                    t.OrganizationId == team.OrganizationId.Value || t.TeamId == team.OrganizationId.Value, t => t.League);
            }
        }

        public Task<IEnumerable<Team>> GetOrganizationById(byte organizationId)
        {
            return _unitOfWork.Teams.FindAsync(t => t.Organization,t =>
                t.OrganizationId == organizationId || t.TeamId == organizationId, t => t.League);
        }

        public Task<List<Team>> GetLeagueTeamsAsync(byte league)
        {
            return _unitOfWork.Teams.FindAsync(t => t.Organization, t => t.League == league);
        }

        public async Task<IEnumerable<Division>> GetLeagueDivisionsAsync(byte league)
        {
            var teams = await GetLeagueTeamsAsync(league);
            var divisions = from team in teams
                group team by team.Division
                into division
                select new Division {DivisionId = division.Key.GetValueOrDefault(), League = league,Teams = division.ToList()};
            return divisions;
        }

        public Task<Team> GetTeamByIdAsync(byte teamId)
        {
            return _unitOfWork.Teams.GetAsync(t=> t.Organization, t => t.TeamId == teamId);
        }
    }
}
