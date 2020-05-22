using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public class TeamsHelper
    {
        private readonly IList<Team> _teams;
        public TeamsHelper(ITeamsService teamsService)
        {
            _teams = teamsService.GetTeamsAsync().Result.ToList();
        }

        public IList<Team> Teams
        {
            get => _teams;
        }

        public Team GetActualTeam(byte teamId, byte league)
        {
            var team = _teams.Single(t => t.TeamId == teamId);
            if (league == team.League)
            {
                return team;
            }

            if (team.OrganizationId == null && team.League == 0)
                return _teams.Single(t => t.OrganizationId == team.TeamId && t.League == league);
    
            var realTeam = _teams.SingleOrDefault(t => t.OrganizationId == team.OrganizationId && t.League == league);
            if (realTeam != null)
                return realTeam;
            else // is MLB
                return _teams.Single(t => t.TeamId == team.OrganizationId && t.League == 0);
        }

    }
}
