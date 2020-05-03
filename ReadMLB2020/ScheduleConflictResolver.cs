using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    internal class MatchTeamsResolver
    {
        private IList<Team> _teams;
        private HtmlDocument _html;

        private const byte NYY = 64;
        private const byte NYM = 63;
        private const byte CWS = 30;
        private const byte ChC = 32;
        private const byte LAA = 20;
        private const byte LA = 4;

        public MatchTeamsResolver(string htmlSource)
        {
            _html = new HtmlDocument();
            _html.Load(htmlSource);
        }

        public async Task InitializeAsync(ITeamsService teamsService)
        {
            _teams = (await teamsService.GetTeamsAsync()).ToList(); ;
        }

        public MatchResult SolveTeams(string homeCity, string awayCity, MatchResult match)
        {
            Team homeTeam = null;
            Team awayTeam = null;
            var homeFoundTeams = _teams.Where(t => t.CityName == homeCity && t.League == match.League).ToList();
            if (homeFoundTeams.Count == 1)
            {
                homeTeam = homeFoundTeams.Single();
                match.HomeTeamId = homeTeam.TeamId;
            }
            var awayFoundTeams = _teams.Where(t => t.CityName == awayCity && t.League == match.League).ToList();
            if (awayFoundTeams.Count == 1)
            {
                awayTeam = awayFoundTeams.Single();
                match.AwayTeamId = awayTeam.TeamId;
            }

            if (homeTeam != null && awayTeam != null)
            {
                return match;
            }

            if (homeTeam == null) //resolve home
            {
                var rivals = CityRivals(homeCity);
                match.HomeTeamId = ResolveRival(rivals[0], rivals[1], match.DateTime, awayCity,
                    new Score {RivalScore = match.AwayScore, TeamScore = match.HomeScore}, true);


            } 
            
            if (awayTeam == null) //resolve away
            {
                var rivals = CityRivals(awayCity);
                match.AwayTeamId = ResolveRival(rivals[0], rivals[1], match.DateTime, homeCity,
                    new Score { RivalScore = match.HomeScore, TeamScore = match.AwayScore }, false);
            }

            return match;
        }

        private byte[] CityRivals(string cityName)
        {
            switch (cityName)
            {
                case "New York":
                    return new byte[] {NYY, NYM};
                case "Chicago":
                    return new byte[] {CWS, ChC};
                case "Los Angeles":
                    return new byte[] {LAA, LA};
                default:
                    throw new NotImplementedException($"Unknown city {cityName}");
            }
        }

        private MatchTemp FindTeamMatchForDay(byte teamId, DateTime matchDate, string rival)
        {
            var scheduleTable = ScheduleHelper.FindScheduleTable(_html, teamId);

            foreach (var row in scheduleTable.SelectNodes("./tr").Skip(1))
            {
                var dateTime = ScheduleHelper.ExtractDate(row);
                var matchRival = ScheduleHelper.ExtractRival(row);
                var isAtHome = ScheduleHelper.IsAtHome(ref matchRival);
                if (dateTime == matchDate && (string.IsNullOrEmpty(rival) || matchRival == rival))
                {
                    return new MatchTemp
                    {
                        IsAtHome = isAtHome,
                        Rival = matchRival,
                        Score = ScheduleHelper.ExtractScore(row),
                        TeamId = teamId
                    };
                }
            }

            return null;
        }


        private byte ResolveRival(byte alTeamId, byte nlTeamId, DateTime matchDate, string rival, Score matchScore, bool isAtHome)
        {
            var al = FindTeamMatchForDay(alTeamId, matchDate, rival);
            var nl = FindTeamMatchForDay(nlTeamId, matchDate, rival);
            if (al == null && nl == null)
                throw new ArgumentException($"No Matches for either team on {matchDate} {alTeamId}/{nlTeamId}");

            if (al != null && nl == null) //It's AL Team, verify anyway
            {
                if (al.Rival != rival || al.Score.TeamScore != matchScore.TeamScore || al.Score.RivalScore != matchScore.RivalScore || al.IsAtHome != isAtHome)
                    throw new ArgumentException($"Match is wrong {rival} vs {alTeamId} on  {matchDate}");
                return alTeamId;
            }
            else if (al == null) //It's NL Team, verify anyway
            {
                if (nl.Rival != rival || nl.Score.TeamScore != matchScore.TeamScore || nl.Score.RivalScore != matchScore.RivalScore || nl.IsAtHome != isAtHome)
                    throw new ArgumentException($"Match is wrong {rival} vs {nlTeamId} on  {matchDate}");
                return nlTeamId;
            }

            //both have value, return the one that has same score
            //unless both have same score!
            if (al.Score.TeamScore == nl.Score.TeamScore && al.Score.RivalScore == nl.Score.RivalScore && al.IsAtHome == nl.IsAtHome)
                throw new ArgumentException($"Both {alTeamId} and {nlTeamId} matches have same date and score {matchDate}");
            if (al.Score.RivalScore == matchScore.RivalScore && al.Score.TeamScore == matchScore.TeamScore && isAtHome == al.IsAtHome)
                return alTeamId;
            return nlTeamId;
        }

    }
}