using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public class ReadRoster
    {
        private readonly short _year;
        private readonly bool _inPO;
        private readonly string _rosterSource;
        private readonly string _rosterTemp;
        private readonly FindPlayer _findPlayerHelper;
        private readonly IRostersService _rostersService;
        private readonly IRotationsService _rotationsService;
        private readonly string _htmlSource;
        private readonly string _rotationTemp;

        public ReadRoster(IConfiguration config, short year, FindPlayer findPlayer, IRostersService rostersService,
            IRotationsService rotationsService, bool inPO, string sourceFile)
        {
            _year = year;
            _inPO = inPO;
            _rosterSource = sourceFile;
            _rosterTemp = Path.Combine(config["SourceFolder"], config["RostersTemp"]);
            _rotationTemp = Path.Combine(config["SourceFolder"], config["RotationsTemp"]);
            _findPlayerHelper = findPlayer;
            _rostersService = rostersService;
            _rotationsService = rotationsService;
            _htmlSource = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}.html");
        }

        internal void ParseRoster()
        {
            Console.WriteLine("Parsing Roster");
            ReadHelper.ReadList(_rosterSource, "\"Roster and Assignment Details\"", 0, 4, 5, false, _rosterTemp, true,
                "\"ROS\"");
            Console.WriteLine("Parse Roster completed");
        }

        internal void ParseRotation()
        {
            Console.WriteLine("Parsing Rotation");
            ReadHelper.ReadList(_rosterSource, "\"Roster and Assignment Details\"", 0, 4, 5, false, _rotationTemp, true,
                "\"ROT\"");
            Console.WriteLine("Parse Roster completed");
        }

        public async Task ReadRostersAsync(IList<Player> players, IList<Team> teams)
        {
            await _rostersService.CleanYearAsync(_year, _inPO);
            Console.WriteLine("Read Rosters");
            using (var file = new StreamReader(_rosterTemp))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var player = await _findPlayerHelper.FindPlayerByName(players, attrs[4].ExtractName(),
                        attrs[5].ExtractName(),
                        _year, Convert.ToByte(attrs[0]));
                    if (player != null)
                    {
                        var slot = new RosterPosition
                        {
                            TeamId = Convert.ToByte(attrs[0]),
                            Slot = Convert.ToByte(attrs[3]),
                            PlayerId = player.PlayerId,
                            Year = _year,
                            League =
                                teams.Single(t => t.TeamId == Convert.ToByte(attrs[0])).League.GetValueOrDefault(),
                            InPO = _inPO
                        };
                        //Console.WriteLine("{0} {1} {2}", slot.Slot, player.FirstName, player.LastName);
                        await _rostersService.AddRosterAsync(slot);
                    }
                    else
                    {
                        Console.WriteLine("Player not found {0} {1}", attrs[4].ExtractName(), attrs[5].ExtractName());
                    }
                }

                file.Close();
            }

            Console.WriteLine("Rosters Completed");
        }

        public async Task ValidatePlayersAsync(IList<Player> players)
        {
            Console.WriteLine("Validate Positions");
            var wrongPositions = new List<PlayerValidation>();
            byte? currentTeam = null;
            using (var file = new StreamReader(_rosterTemp))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var slot = new PlayerValidation
                    {
                        TeamId = Convert.ToByte(attrs[0]), Slot = Convert.ToByte(attrs[3]),
                        FirstName = attrs[4].ExtractName(), LastName = attrs[5].ExtractName()
                    };
                    if (currentTeam.HasValue || slot.TeamId != currentTeam.GetValueOrDefault())
                        currentTeam = slot.TeamId;
                    var player = await _findPlayerHelper.FindPlayerByName(players, slot.FirstName, slot.LastName, _year,
                        currentTeam);
                    if (player == null)
                    {
                        wrongPositions.Add(slot);
                    }

                    //Console.WriteLine("{0} {1} {2}", slot.Slot, slot.FirstName, slot.LastName);
                }

                file.Close();
            }

            //those who are just once in the slots are just duplicated names apparently
            var groupByName = wrongPositions.GroupBy(s => new {s.FirstName, s.LastName})
                .Select(g => new {Slot = g.Key, Count = g.Count()});

            wrongPositions = wrongPositions.Where(s =>
                groupByName.Where(g => g.Count > 1)
                    .Any(sl => sl.Slot.FirstName == s.FirstName && sl.Slot.LastName == s.LastName)).ToList();

            foreach (var pos in wrongPositions.OrderBy(wp => wp.FirstName).ThenBy(wp => wp.LastName))
            {
                Console.WriteLine("Wrong: {0} {1} - {2}/{3}", pos.FirstName, pos.LastName, pos.TeamId, pos.Slot);
            }
        }


        public async Task ReadRotationsAsync(IList<Player> players, IList<Team> teams)
        {
            await _rotationsService.CleanYearAsync(_year, _inPO);
            Console.WriteLine("Read Rotations");
            using (var file = new StreamReader(_rotationTemp))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    


                    var player = await _findPlayerHelper.FindPitcherByName(players, attrs[4].ExtractName(),
                        attrs[5].ExtractName(), _year);
                    if (player == null)
                    {
                        player = await _findPlayerHelper.FindPlayerByName(players, attrs[4].ExtractName(),
                            attrs[5].ExtractName(),
                            _year, Convert.ToByte(attrs[0]));
                    }

                    if (player != null)
                    {
                        var team = teams.Single(t => t.TeamId == Convert.ToByte(attrs[0]));
                        await _rotationsService.AddRotationPositionAsync(new RotationPosition
                        {
                            TeamId = team.TeamId, 
                            InPO = _inPO, 
                            Year = _year, 
                            PlayerId = player.PlayerId,
                            League = team.League.GetValueOrDefault(),
                            Slot = Convert.ToByte(attrs[3]),
                            PitcherAssignment = PitcherAssignment.Rotation
                        });
                    }
                    else
                    {
                        Console.WriteLine("Player not found {0} {1}", attrs[4].ExtractName(), attrs[5].ExtractName());
                    }
                }
            }
            Console.WriteLine("Finished Rotations");
        }

        public async Task ReadPitcherAssignmentAsync(IList<Team> teams)
        {
            Console.WriteLine("Updating Pitching assignment");
            await _rotationsService.CleanYearAsync(_year, _inPO);
            var html = new HtmlDocument();
            html.Load(_htmlSource);

            foreach (var team in teams)
            {
                //anchor with teamId is below a b, then there's a br, then a text? then something, then the table
                var paTable = html.DocumentNode.SelectSingleNode($"//b/a[@name='t{team.TeamId}']").ParentNode
                    .NextSibling
                    .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                //validate is the roster
                if (paTable.FirstChild.FirstChild.InnerHtml != "Pitching Assignments")
                    throw new FormatException("Pitching Assignments table not found, or found wrong table.");
                //get team's roster
                var roster = (await _rostersService.GetTeamRosterAsync(team.TeamId, _year, _inPO)).ToList();

                var row = paTable.SelectNodes("./tr").Skip(2).First();
                var assignments = row.SelectNodes("./td/small");
                var rotation = new List<RotationPosition>();
                byte slot = 0;

                //Closer = 5, Rotation = 0
                for (int paIndex = 0; paIndex < 5  ; paIndex++)
                {
                    var pitchers = assignments[paIndex].InnerHtml.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var pitcher in pitchers)
                    {
                        var names = pitcher.Replace("<br>", "").Split(',');
                        var players = roster.Where(p =>
                            p.Player.FirstName == names[1].TrimStart() &&
                            p.Player.LastName == names[0]);

                        if (players.Count() != 1)
                            Console.WriteLine("Player not found {0} {1} in team {2}",
                                names[1],names[0], team.TeamId);
                        else
                        {
                            var player = players.Single().Player;
                            if (rotation.All(p => p.PlayerId != player.PlayerId))
                            {
                                //Console.WriteLine("Pitcher {0} slot {1} {2} {3}", (PitcherAssignment)paIndex, slot, names[1], names[0]);
                                try
                                {
                                    var pa = new RotationPosition
                                    {
                                        TeamId = team.TeamId,
                                        Year = _year,
                                        League = team.League.GetValueOrDefault(),
                                        InPO = _inPO,
                                        PlayerId = player.PlayerId,
                                        PitcherAssignment = (PitcherAssignment) paIndex,
                                        Slot = slot++
                                    };
                                    rotation.Add(pa);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error");
                                }
                            }
                        }
                    }

                }
                await _rotationsService.AddTeamRotationAsync(rotation);
            }

            Console.WriteLine("Pitching assignment finished");
        }
    }
}
