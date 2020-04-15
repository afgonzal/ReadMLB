using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMLB2020
{
    public class ReadBatting
    {
        private readonly bool _inPO;
        private readonly short _year;
        private readonly string _battingSource;
        private readonly string _battingTemp;
        private readonly string _battingStats;
        private readonly IBattingService _battingService;


        public ReadBatting(IBattingService battingService, IConfiguration config, short year)
        {
            _inPO = false;
            _year = year;
            _battingSource = Path.Combine(config["SourceFolder"], config["SourceFile"]);
            _battingTemp = Path.Combine(config["SourceFolder"], config["BattingTempStats"]);
            _battingStats = Path.Combine(config["SourceFolder"], config["BattingStats"]);
            _battingService = battingService;
        }

        private List<Batting> ParseBattingStats()
        {
            var stats = new List<Batting>();
            using (var file = new StreamReader(_battingStats))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    stats.Add(new Batting
                    {
                        PlayerId = Convert.ToInt64(attrs[1]),
                        TeamId = (attrs[4] == "-1") ? (byte)100 : Convert.ToInt16(attrs[4]),
                        League = Convert.ToByte(attrs[5]),
                        G = Convert.ToInt16(attrs[6]),
                        BattingVs = BattingVs.Total,
                        PA = Convert.ToInt16(attrs[13]),
                        H1B = Convert.ToInt16(attrs[14]),
                        H2B = Convert.ToInt16(attrs[15]),
                        H3B = Convert.ToInt16(attrs[16]),
                        HR = Convert.ToInt16(attrs[17]),
                        RBI = Convert.ToInt16(attrs[18]),
                        SO = Convert.ToInt16(attrs[19]),
                        BB = Convert.ToInt16(attrs[20]),
                        SH = Convert.ToInt16(attrs[21]),
                        SF = Convert.ToInt16(attrs[22]),
                        IBB = Convert.ToInt16(attrs[23]),
                        HBP = Convert.ToInt16(attrs[24]),
                        InPO = _inPO,
                        Year = _year
                    });;
                }
                file.Close();
            }
            return stats;
        }


        public void ParseBatting()
        {
            Console.WriteLine("Reading Batting");
            ReadHelper.ReadList(_battingSource, "\"Batting Statistics - Majors\"", 4, 1, 2, true, _battingTemp);
            Console.WriteLine("Read Batting completed");
        }
        public async Task UpdateBattingStatsAsync(IEnumerable<Player> players)
        {
            await _battingService.TruncateBattingStatsAsync();
            var bStats = ParseBattingStats();
            //Iterate Battting Temp
            using (var file = new StreamReader(_battingTemp))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var player = players.Single(p => p.PlayerNumerator == Convert.ToInt32(attrs[0]));
                    try
                    {
                        var statsMajor = bStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 0);
                        var statsAAA = bStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 1);
                        var statsAA = bStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 2);
                       
                        if (statsMajor != null && statsMajor.PA > 0)
                        {
                            await _battingService.AddBattingStatAsync(statsMajor);
                            var vsLeft = new Batting
                            {
                                PA = Convert.ToInt16(attrs[15]),
                                H1B = Convert.ToInt16(attrs[16]),
                                H2B = Convert.ToInt16(attrs[17]),
                                H3B = Convert.ToInt16(attrs[18]),
                                HR = Convert.ToInt16(attrs[19]),
                                RBI = Convert.ToInt16(attrs[20]),
                                SO = Convert.ToInt16(attrs[21]),
                                BB = Convert.ToInt16(attrs[22]),
                                G = statsMajor.G,
                                BattingVs = BattingVs.Left,
                                InPO = _inPO,
                                League = 0,
                                TeamId = statsMajor.TeamId,
                                PlayerId = player.PlayerId,
                                Year = _year
                            };
                            var vsRight = new Batting
                            {
                                PA = Convert.ToInt16(attrs[23]),
                                H1B = Convert.ToInt16(attrs[24]),
                                H2B = Convert.ToInt16(attrs[25]),
                                H3B = Convert.ToInt16(attrs[26]),
                                HR = Convert.ToInt16(attrs[27]),
                                RBI = Convert.ToInt16(attrs[28]),
                                SO = Convert.ToInt16(attrs[29]),
                                BB = Convert.ToInt16(attrs[30]),
                                G = statsMajor.G,
                                BattingVs = BattingVs.Right,
                                InPO = _inPO,
                                League = 0,
                                TeamId = statsMajor.TeamId,
                                PlayerId = player.PlayerId,
                                Year = _year
                            };
                            await _battingService.AddBattingStatAsync(vsLeft);
                            await _battingService.AddBattingStatAsync(vsRight);

                        }
                        if (statsAAA != null)
                            await _battingService.AddBattingStatAsync(statsAAA);
                        if (statsAA != null)
                            await _battingService.AddBattingStatAsync(statsAA);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed for {0}", player.PlayerId);
                    }

                }
                file.Close();
            }
            Console.WriteLine("Batting parse complete.");
        }
    }
}
