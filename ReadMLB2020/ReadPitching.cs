using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReadMLB2020
{
    public class ReadPitching
    {
        private readonly IPitchingService _pitchingService;
        private readonly short _year;
        private readonly bool _inPo;
        private readonly string _pitchingSource;
        private readonly string _pitchingTemp;
        private readonly string _pitchingStats;
        private FindPlayer _findPlayerHelper;
        private readonly IRostersService _rosterService;

        public ReadPitching(IPitchingService pitchingService, IRostersService rostersService, FindPlayer findPlayer, IConfiguration config, short year, bool inPO, string sourceFile)
        {
            _pitchingService = pitchingService;
            _year = year;
            _inPo = inPO;
            _pitchingSource = sourceFile;
            _pitchingTemp = Path.Combine(config["SourceFolder"], config["PitchingTempStats"]);
            _pitchingStats = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}{config["PitchingStats"]}");
            _findPlayerHelper = findPlayer;
            _rosterService = rostersService;
        }

        internal void ParsePitching()
        {
            Console.WriteLine("Reading Pitching");
            ReadHelper.ReadList(_pitchingSource, "\"Pitching Statistics - Majors\"", 2, 0, 1, false, _pitchingTemp);
            Console.WriteLine("Read Pitching completed");
        }
        private IList<Pitching> ParsePitchingStats()
        {
            var stats = new List<Pitching>();
            using (var file = new StreamReader(_pitchingStats))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    stats.Add(new Pitching
                    {
                        PlayerId = Convert.ToInt64(attrs[1]),
                        TeamId = (attrs[2] == "-1") ? (byte)100 : Convert.ToByte(attrs[2]),
                        League = Convert.ToByte(attrs[3]),
                        G = Convert.ToInt16(attrs[4]),
                        GS = Convert.ToInt16(attrs[5]),
                        IP10 = Convert.ToInt16(attrs[6]),
                        W = Convert.ToInt16(attrs[7]),
                        L = Convert.ToInt16(attrs[8]),  
                        SV = Convert.ToInt16(attrs[9]),
                        BSV = Convert.ToInt16(attrs[10]),
                        CG = Convert.ToInt16(attrs[11]),
                        SHO = Convert.ToInt16(attrs[12]),
                        R = Convert.ToInt16(attrs[13]),
                        ER = Convert.ToInt16(attrs[14]),
                        K = Convert.ToInt16(attrs[15]),
                        BB = Convert.ToInt16(attrs[16]),
                        H = Convert.ToInt16(attrs[17]),
                        HR = Convert.ToInt16(attrs[18]),

                        InPO = _inPo,
                        Year = _year
                    }); ;
                }
                file.Close();
            }
            return stats;
        }

        private class PitchingTempStat :  Pitching
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private IList<PitchingTempStat> ParsePitchingFile()
        {
            var stats = new List<PitchingTempStat>();
            using (var file = new StreamReader(_pitchingTemp))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    if (Convert.ToInt16(attrs[2]) > 0)
                    {
                        stats.Add(new PitchingTempStat
                        {
                            PK = Convert.ToInt16(attrs[13]),
                            TPA = Convert.ToInt16(attrs[14]),
                            H1B = Convert.ToInt16(attrs[15]),
                            H2B = Convert.ToInt16(attrs[16]),
                            H3B = Convert.ToInt16(attrs[17]),
                            IBB = Convert.ToInt16(attrs[21]),
                            HB = Convert.ToInt16(attrs[22]),
                            SF = Convert.ToInt16(attrs[23]),
                            SH = Convert.ToInt16(attrs[24]),
                            FirstName = attrs[0].ExtractName(),
                            LastName = attrs[1].ExtractName()
                        });
                    }
                }
                file.Close();
            }
            return stats;
        }

        internal async Task UpdatePitchingStatsAsync(IList<Player> players, IList<Team> teams)
        {
            Console.WriteLine("Update Pitching stats.");
            await _pitchingService.CleanYearAsync(_year, _inPo);
            var pStats = ParsePitchingFile();

            //Iterate Pitch Temp
            using (var file = new StreamReader(_pitchingStats))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                        var stats = new Pitching
                        {
                            PlayerId = Convert.ToInt64(attrs[1]),
                            TeamId = (attrs[2] == "-1") ? (byte)100 : Convert.ToByte(attrs[2]),
                            League = Convert.ToByte(attrs[3]),
                            G = Convert.ToInt16(attrs[4]),
                            GS = Convert.ToInt16(attrs[5]),
                            IP10 = Convert.ToInt16(attrs[6]),
                            W = Convert.ToInt16(attrs[7]),
                            L = Convert.ToInt16(attrs[8]),
                            SV = Convert.ToInt16(attrs[9]),
                            BSV = Convert.ToInt16(attrs[10]),
                            CG = Convert.ToInt16(attrs[11]),
                            SHO = Convert.ToInt16(attrs[12]),
                            R = Convert.ToInt16(attrs[13]),
                            ER = Convert.ToInt16(attrs[14]),
                            K = Convert.ToInt16(attrs[15]),
                            BB = Convert.ToInt16(attrs[16]),
                            H = Convert.ToInt16(attrs[17]),
                            HR = Convert.ToInt16(attrs[18]),

                            InPO = _inPo,
                            Year = _year
                        };
                        var player = _findPlayerHelper.FindPlayerById(players, stats.PlayerId, _year);
                        if (player == null)
                        {
                            Console.WriteLine("Can't match player {0}", stats.PlayerId);
                        }
                        else
                        {
                            stats.PlayerId = player.PlayerId;
                            var tStats = pStats.Where(s =>
                                s.FirstName == player.FirstName && s.LastName == player.LastName).ToList();
                            if (tStats.Count > 1) //several pitchers with same name
                            {
                                Console.WriteLine("Several pitchers with same name {0} {1}", player.FirstName, player.LastName);
                            }
                            else if (tStats.Count == 1) //add extra info from pitching.txt
                            {
                                var extraStats = tStats.First();
                                stats.PK = extraStats.PK;
                                stats.TPA = extraStats.TPA;
                                stats.H1B = extraStats.H1B;
                                stats.H2B = extraStats.H2B;
                                stats.H3B = extraStats.H3B;
                                stats.IBB = extraStats.IBB;
                                stats.HB = extraStats.HB;
                                stats.SF = extraStats.SF;
                                stats.SH = extraStats.SH;
                            }
                            await _pitchingService.AddPitchingStatAsync(stats);
                    }
                }
                file.Close();
            }
            Console.WriteLine("Pitching parse completed");
        }
    }
}
