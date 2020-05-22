using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReadMLB.Entities;

namespace ReadMLB2020
{
    internal class MatchTemp 
    {
        public byte TeamId { get; set; }
        public string Rival { get; set; }
        public Score Score { get; set; }
        public bool IsAtHome { get; set; }
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

    public class ReadSchedule
    {
        private ITeamsService _teamsService;
        private IScheduleService _scheduleService;
        private short _year;
        private bool _inPO;
        private readonly string _htmlSource;
        private readonly string _scheduleSource;
        private string _scheduleTemp;
        private IList<MatchResult> _fullSchedule = new List<MatchResult>();

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
            ReadHelper.ReadList(_scheduleSource, "\"Schedule Data - Majors\"", 0, 2, 3, false, _scheduleTemp);
            Console.WriteLine("Parse players complete.");
        }

        public Task CleanScheduleAsync()
        {
            return _scheduleService.CleanYearAsync(_year, _inPO);
        }

       public async Task UpdateScheduleAsync(byte league)
        {
            Console.WriteLine("Updating Schedule");
            var sw = new Stopwatch();
            sw.Start();
            var resolver = new MatchTeamsResolver(_htmlSource);
            await resolver.InitializeAsync(_teamsService);
            using (var file = new StreamReader(_scheduleTemp))
            {
                string line;
                bool keepReading = true;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var currentLeague = Convert.ToByte(attrs[0]);
                    if (currentLeague > league)
                    {
                        keepReading = false;
                        continue;
                    }
                    else if (currentLeague < league)
                    {
                        continue;
                    }

                    var match = new MatchResult
                        {
                            DateTime = DateTime.ParseExact(attrs[1].ExtractName(), "MM/dd/yyyy hh:mm tt", null),
                            HomeScore = Convert.ToByte(attrs[3]),
                            AwayScore = Convert.ToByte(attrs[5]),
                            Year = _year,
                            InPO =  _inPO,
                            Round = "R",
                            League = league
                        };
                    //check if it's all star game
                    if (attrs[2].ExtractName() == "American")
                    {
                        match.HomeTeamId = 0;
                        match.AwayTeamId = 8;
                    } else if (attrs[2].ExtractName() == "National")
                    {
                        match.HomeTeamId = 8;
                        match.AwayTeamId = 0;
                    }
                    else //not the All star
                    {
                        match = resolver.SolveTeams(attrs[2].ExtractName(), attrs[4].ExtractName(), match);
                    }
                    _fullSchedule.Add(match);
                }
            }

            sw.Stop();
            Console.WriteLine("Scheduled completed {0} matches in {1} for league {2}", _fullSchedule.Count, sw.Elapsed.TotalSeconds, league);
        }

       public Task WriteToDbAsync()
       {
           return _scheduleService.AddScheduleAsync(_fullSchedule);
       }

       public async Task UpdatePlayoffsAsync(byte league)
       {
           Console.WriteLine("Updating Playoffs");
           var resolver = new MatchTeamsResolver(_htmlSource);
           await resolver.InitializeAsync(_teamsService);
           using (var file = new StreamReader(_scheduleTemp))
           {
               string line;
               bool keepReading = true;
               while ((line = await file.ReadLineAsync()) != null)
               {
                   var attrs = line.Split(ReadHelper.Separator);
                   var currentLeague = Convert.ToByte(attrs[0]);
                   if (currentLeague > league)
                   {
                       keepReading = false;
                       continue;
                   }
                   else if (currentLeague < league)
                   {
                       continue;
                   }

                   var match = new MatchResult
                   {
                       DateTime = DateTime.ParseExact(attrs[1].ExtractName(), "MM/dd/yyyy hh:mm tt", null),
                       HomeScore = Convert.ToByte(attrs[3]),
                       AwayScore = Convert.ToByte(attrs[5]),
                       Year = _year,
                       InPO = _inPO,
                       League = league
                   };
                   match = resolver.SolveTeams(attrs[2].ExtractName(), attrs[4].ExtractName(), match);

                   //exception case
                   //bye round in AA-SL
                   if (match.HomeTeamId != match.AwayTeamId)
                        _fullSchedule.Add(match);
               }
           }

           SolveRounds(league);
           Console.WriteLine("Finished Playoffs");
       }

       private byte NumberOfWinsPerRound(byte league, byte round)
       {
           switch (league)
           {
                case 0:
                    return round == 1 ? (byte)3 : (byte)4;
                case 1:
                    return 3;
                case 2:
                    return 3;
                default: 
                    throw new NotImplementedException("Wrong league and round");
           }
       }

       private void SolveRounds(byte league)
       {
           var teams = _teamsService.GetTeamsAsync().Result.ToList();
           while (_fullSchedule.Any(m => string.IsNullOrEmpty(m.Round)))
           {
                //get the first match and start
                var firstMatch = _fullSchedule.First(m => string.IsNullOrEmpty(m.Round));
                var teamMatches = _fullSchedule.Where(m => m.HomeTeamId == firstMatch.HomeTeamId || m.AwayTeamId == firstMatch.HomeTeamId);
                byte round = 1;
                var wins = 0;
                foreach (var match in teamMatches)
                {
                    match.Round = round == 1 ? "DS" : round == 2 ? "CS" : league == 0 ? "WS" : "CS"; //no WS in MiLB
                    Console.WriteLine("{0} {1} vs {2}",match, teams.Single(t=> t.TeamId == match.HomeTeamId).TeamName, teams.Single(t => t.TeamId == match.AwayTeamId).TeamName);
                    if (match.WoL(firstMatch.HomeTeamId) == 'W')
                        wins++;

                    if (wins == NumberOfWinsPerRound(league, round))
                    {
                        round++;
                        wins = 0;
                    }
                }
           }
        }
    }
}
