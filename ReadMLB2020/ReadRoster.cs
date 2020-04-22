using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private FindPlayer _findPlayerHelper;
        private readonly IRostersService _rostersService;
        private readonly IRotationsService _rotationsService;
        private string _rotationTemp;

        public ReadRoster(IConfiguration config, short year, FindPlayer findPlayer, IRostersService rostersService,
            IRotationsService rotationsService, bool inPO)
        {
            _year = year;
            _inPO = inPO;
            _rosterSource = Path.Combine(config["SourceFolder"], config["SourceFile"]);
            _rosterTemp = Path.Combine(config["SourceFolder"], config["RostersTemp"]);
            _rotationTemp = Path.Combine(config["SourceFolder"], config["RotationsTemp"]);
            _findPlayerHelper = findPlayer;
            _rostersService = rostersService;
            _rotationsService = rotationsService;
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
                        try
                        {


                            await _rostersService.AddRosterAsync(slot);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error for {0} {1}", attrs[4].ExtractName(),
                                attrs[5].ExtractName());
                        }
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
                        attrs[5].ExtractName(), _year, Convert.ToByte(attrs[0]));
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
                            TeamId = team.TeamId, InPO = _inPO, Year = _year, PlayerId = player.PlayerId,
                            League = team.League.GetValueOrDefault(), Slot = Convert.ToByte(attrs[3])
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
    }
}
