using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ReadMLB.Entities;

namespace ReadMLB2020
{
    internal class MatchTemp 
    {
        public byte TeamId { get; set; }
        public string Rival { get; set; }
        public Score Score { get; set; }
    }
    internal struct Score
    {
        public byte TeamScore;
        public byte RivalScore;
        public bool W;

        public char Result => W ? 'W' : 'L';

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = (Score) obj;

            return RivalScore == other.RivalScore && TeamScore == other.TeamScore;
        }
    }
    internal class ScheduleHelper
    {
        private const string ScoreEx = "^[WL], \\d+-\\d+$";
        public static string ExtractRival(HtmlNode row)
        {
            return row.ChildNodes[1].InnerHtml.TrimEnd();
        }

        public static Score ExtractScore(HtmlNode row)
        {
            var result = row.ChildNodes[2].InnerHtml;
            var w = Regex.Match(result, "^[WL]").Value == "W" ? true : false;
            var teamScore = Convert.ToByte(Regex.Match(result, "\\d-").Value.Replace('-',' '));
            var rivalScore = Convert.ToByte(Regex.Match(result, "\\d$").Value);

            return new Score { RivalScore =  rivalScore, TeamScore = teamScore, W = w};
        }

        public static DateTime ExtractDate(HtmlNode row)
        {
            return DateTime.ParseExact(row.ChildNodes[0].ChildNodes[0].InnerHtml, "MM/dd/yyyy hh:mm tt",
                null);
        }

        public static HtmlNode FindScheduleTable(HtmlDocument html, byte teamId)
        {
            var node = html.DocumentNode.SelectSingleNode($"//b/a[@name='t{teamId}']").ParentNode;

            do
            {
                node = node.NextSibling;
            } while (node.Name != "table" || node.FirstChild?.FirstChild == null || node.FirstChild.FirstChild.InnerHtml != "Date/Time");

            return node;

        }

        public static bool IsAtHome(ref string rival)
        {
            if (rival.StartsWith("at "))
            {
                rival = rival.Substring(3);
                return false;
            }

            return true;
        }
    }
    public class ReadSchedule
    {
        private ITeamsService _teamsService;
        private IScheduleService _scheduleService;
        private short _year;
        private bool _inPO;
        private readonly string _htmlSource;
        private readonly string _scheduleSource;
        private string _scheduleTemp;

        public ReadSchedule(ITeamsService teamsService, IScheduleService scheduleService, IConfiguration config, short year, bool inPO, string sourceFile)
        {
            _teamsService = teamsService;
            _scheduleService = scheduleService;
            _year = year;
            _inPO = inPO;
            _htmlSource = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}.html");
            _scheduleSource = sourceFile;
            _scheduleTemp = Path.Combine(config["SourceFolder"], config["ScheduleTemp"]);
        }

        public void ParseSchedule()
        {
            Console.WriteLine("Reading Players");
            ReadHelper.ReadList(_scheduleSource, "\"Schedule Data - Majors\"", 1, 2, 3, false, _scheduleTemp);
            Console.WriteLine("Parse players complete.");
        }

        public async Task UpdateScheduleAsync(byte league)
        {
            Console.WriteLine("Updating Schedule");
            var sw  = new Stopwatch();
            sw.Start();
            var teams = (await _teamsService.GetTeamsAsync()).ToList();
            var fullSchedule = new List<MatchResult>();
            var html = new HtmlDocument();
            html.Load(_htmlSource);
            foreach (var team in teams.Where(t => t.League == league))
            {
                var scheduleTable = ScheduleHelper.FindScheduleTable(html, team.TeamId);
               
                var resolver = new ScheduleConflictResolver(team, _htmlSource);
                //validate is the roster
                if (scheduleTable.FirstChild.FirstChild.InnerHtml != "Date/Time")
                    throw new FormatException("Schedule table not found, or found wrong table.");

                foreach (var row in scheduleTable.SelectNodes("./tr").Skip(1))
                {
                    var dateTime = ScheduleHelper.ExtractDate(row);
                    var rivalCity = ScheduleHelper.ExtractRival(row);
                    var isAtHome = ScheduleHelper.IsAtHome(ref rivalCity);

                    var score = ScheduleHelper.ExtractScore(row);
                    

                    Team rival = null;
                    var rivalTeamCandidates = teams.Where(t => t.CityName == rivalCity && t.League == team.League).ToList();
                    if (!rivalTeamCandidates.Any())
                    {
                        Console.WriteLine("City not found {0}", rivalCity);
                        throw new ArgumentException($"City not found {rivalCity}");
                    }
                    else if (rivalTeamCandidates.Count() == 1)
                    {
                        rival = rivalTeamCandidates.Single();
                    }
                    else //multiple cities
                    {
                        //Console.WriteLine("Team match {0} {1}", rivalCity, dateTime);
                        var rivalId = resolver.ResolveRivalForDate(rivalCity, dateTime, score);
                        if (!rivalId.HasValue)
                            Console.WriteLine("Multiple cities for {0} facing team {1}", rivalCity, team.TeamAbr);
                        rival = teams.Single(t => t.TeamId == rivalId);
                    }

                    var match = new MatchResult
                    {
                        DateTime = dateTime,
                        Year = _year,
                        Round = "R",
                        InPO = _inPO,
                        HomeTeamId = isAtHome ? team.TeamId : rival.TeamId,
                        HomeScore = isAtHome ? score.TeamScore : score.RivalScore,
                        AwayTeamId = isAtHome ? rival.TeamId : team.TeamId,
                        AwayScore = isAtHome ? score.RivalScore : score.TeamScore
                    };
                    if (!fullSchedule.Any(m =>
                        m.DateTime == dateTime && m.HomeTeamId == match.HomeTeamId && m.AwayTeamId == match.AwayTeamId))
                    {
                        fullSchedule.Add(match);
                    }
                }
            }

            sw.Stop();
            Console.WriteLine("Scheduled completed {0} matches in {1}", fullSchedule.Count, sw.Elapsed.TotalSeconds);
        }
    }

    internal class ScheduleConflictResolver
    {
        private const byte NYY = 64;
        private const byte NYM = 63;
        private const byte CWS = 30;
        private const byte ChC = 32;
        private const byte LAA = 20;
        private const byte LA = 4;
        private readonly Team _team;
        private readonly string _htmlSource;
        private HtmlDocument _html;

        public ScheduleConflictResolver(Team team, string htmlSource)
        {
            _team = team;
            _htmlSource = htmlSource;
            _html = new HtmlDocument();
            _html.Load(_htmlSource);
        }

        public byte? ResolveRivalForDate(string rivalCity, DateTime matchDate, Score score)
        {
            if (rivalCity == _team.CityName)
            {
                switch (_team.TeamId)
                {
                    case NYY:
                        return NYM;
                    case NYM:
                        return NYY;
                    case CWS:
                        return ChC;
                    case ChC:
                        return CWS;
                    case LAA:
                        return LA;
                    case LA:
                        return LAA;
                    default:
                        throw new NotImplementedException($"No conflict resolver for {_team.TeamId}");
                }
            }
            switch (rivalCity)
            {
                case "New York":
                    return ResolveVsCityTeams(NYY, NYM, matchDate, _team.CityName, score);
                case "Chicago":
                    return ResolveVsCityTeams(CWS, ChC, matchDate, _team.CityName, score);
                case "Los Angeles":
                    return ResolveVsCityTeams(LAA, LA, matchDate, _team.CityName, score);
                default:
                    return null;
            }
        }

       
        private byte ResolveVsCityTeams(byte alTeamId, byte nlTeamId, DateTime matchDate, string rival, Score matchScore)
        {
            var al = FindOppTeamMatchForDay(alTeamId, matchDate, rival);
            var nl = FindOppTeamMatchForDay(nlTeamId, matchDate, rival);
            if (al == null && nl == null)
                throw new ArgumentException($"No Matches for either team on {matchDate} {alTeamId}/{nlTeamId}");

            if (al != null && nl == null) //It's CWS, verify anyway
            {
                if (al.Rival != rival || al.Score.TeamScore != matchScore.RivalScore || al.Score.RivalScore != matchScore.TeamScore)
                    throw new ArgumentException($"Match is wrong {rival} vs {alTeamId} on  {matchDate}");
                return alTeamId;
            }
            else if (al == null) //It's ChC, verify anyway
            {
                if (nl.Rival != rival || nl.Score.TeamScore != matchScore.RivalScore || nl.Score.RivalScore != matchScore.TeamScore)
                    throw new ArgumentException($"Match is wrong {rival} vs {nlTeamId} on  {matchDate}");
                return nlTeamId;
            }
            //both have value, return the one that has same score
            //unless both have same score!
            if (al.Score.TeamScore == nl.Score.TeamScore && al.Score.RivalScore == nl.Score.RivalScore)
                throw new ArgumentException($"Both {alTeamId} and {nlTeamId} matches have same date and score {matchDate}");
            if (al.Score.RivalScore == matchScore.TeamScore && al.Score.TeamScore == matchScore.RivalScore)
                return alTeamId;
            return nlTeamId;
        }

        private MatchTemp FindOppTeamMatchForDay(byte oppTeamId, DateTime matchDate, string rival)
        {

            var scheduleTable = ScheduleHelper.FindScheduleTable(_html, oppTeamId);

            foreach (var row in scheduleTable.SelectNodes("./tr").Skip(1))
            {
                var dateTime = ScheduleHelper.ExtractDate(row);
                var matchRival = ScheduleHelper.ExtractRival(row);
                ScheduleHelper.IsAtHome(ref matchRival);
                if (dateTime == matchDate && matchRival == rival)
                {
                    return new MatchTemp
                    {
                        Rival = matchRival, Score = ScheduleHelper.ExtractScore(row),
                        TeamId = oppTeamId
                    };
                }
            }

            return null;
        }

        
    }
}
