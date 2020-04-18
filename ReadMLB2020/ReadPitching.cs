using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
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

        public ReadPitching(IPitchingService pitchingService, IRostersService rostersService, FindPlayer findPlayer, IConfiguration config, short year)
        {
            _pitchingService = pitchingService;
            _year = year;
            _inPo = false;
            _pitchingSource = Path.Combine(config["SourceFolder"], config["SourceFile"]);
            _pitchingTemp = Path.Combine(config["SourceFolder"], config["PitchingTempStats"]);
            _pitchingStats = Path.Combine(config["SourceFolder"], config["PitchingStats"]);
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
                        IP = Convert.ToSingle(attrs[6]),
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


        internal async Task UpdatePitchingStatsAsync(IList<Player> players, IList<Team> teams)
        {
            Console.WriteLine("Update Pitching stats.");
            await _pitchingService.CleanYearAsync(_year);
            var pStats = ParsePitchingStats();

            //Iterate Pitch Temp
            using (var file = new StreamReader(_pitchingTemp))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    try
                    {
                        Player player = null;
                        //var player = await _findPlayerHelper.FindPlayerByName(players, attrs[0].ExtractName(), attrs[1].ExtractName(), _year);
                        var found = players.Where(p =>
                            p.FirstName == attrs[0].ExtractName() && p.LastName == attrs[1].ExtractName()).ToList();
                        
                        if (found.Count > 1)
                        {
                            var foundPitchers = new List<long>();
                            foreach (var fPlayer in found)
                            {
                                var fStats = pStats.Where(p => p.PlayerId == fPlayer.PlayerId);
                                if (fStats.Any()) //it's a pitcher
                                {
                                    foundPitchers.Add(fPlayer.PlayerId);
                                }
                            }

                            if (foundPitchers.Count == 1) //that's him
                            {
                                player = found.Single(p => p.PlayerId == foundPitchers.First());
                            }
                            else //both are pitchers!!!
                            {
                                var pitchersWMatchingStats = new List<long>();
                                //compare stats 
                                foreach (var fPlayer in found)
                                {
                                    foreach (var pstat in pStats.Where(p => p.PlayerId == fPlayer.PlayerId))
                                    {
                                        if (pstat.G == Convert.ToInt16(attrs[2]) &&
                                            pstat.IP.ToString() == attrs[3].ExtractName().Replace(".","") &&
                                            pstat.W == Convert.ToInt16(attrs[4]) &&
                                            pstat.L == Convert.ToInt16(attrs[5]) &&
                                            pstat.SV == Convert.ToInt16(attrs[6]) &&
                                            pstat.BSV == Convert.ToInt16(attrs[7]) &&
                                            pstat.R == Convert.ToInt16(attrs[8]) &&
                                            pstat.ER == Convert.ToInt16(attrs[9]) &&
                                            pstat.GS == Convert.ToInt16(attrs[10]) &&
                                            pstat.CG == Convert.ToInt16(attrs[11]) &&
                                            pstat.SHO == Convert.ToInt16(attrs[12]) &&
                                            pstat.HR == Convert.ToInt16(attrs[18]) &&
                                            pstat.K == Convert.ToInt16(attrs[19]) &&
                                            pstat.BB == Convert.ToInt16(attrs[20]))
                                        {
                                            pitchersWMatchingStats.Add(fPlayer.PlayerId);
                                        }
                                    }
                                }
                                
                                if (pitchersWMatchingStats.Count == 1) //that's him
                                {
                                    player = found.Single(p => p.PlayerId == foundPitchers.First());
                                }
                                else
                                {
                                    if (pitchersWMatchingStats.Count == 0)
                                    {
                                        //Player is in the minors
                                        var matchInMinors = new List<long>();
                                        foreach (var mPlayer in found)
                                        {
                                            //find it in roster
                                            var roster = await _rosterService.FindByPlayerAsync(mPlayer.PlayerId);
                                            if (teams.Single(t => t.TeamId == roster.TeamId).League > 0)
                                            {
                                                matchInMinors.Add(mPlayer.PlayerId);
                                            }
                                        }

                                        if (matchInMinors.Count == 1)
                                            player = found.Single(p => p.PlayerId == matchInMinors.First());
                                        else
                                        {
                                            Console.WriteLine("Multiple matching in minors {0} {1}", attrs[0].ExtractName(),
                                                attrs[1].ExtractName());
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Multiple matching stats {0} {1}", attrs[0].ExtractName(),
                                            attrs[1].ExtractName());
                                    }
                                }
                            }
                        }
                        else //good there's just one player with same name
                        {
                            player = found.First();
                        }

                        if (player != null)
                        {

                            //var statsMajor =
                            //    pStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 0);
                            //var statsAAA = pStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 1);
                            //var statsAA = pStats.SingleOrDefault(s => s.PlayerId == player.PlayerId && s.League == 2);

                            //if (statsMajor != null && statsMajor.G > 0)
                            //{
                            //    statsMajor.PK = Convert.ToInt16(attrs[13]);
                            //    statsMajor.TPA = Convert.ToInt16(attrs[14]);
                            //    statsMajor.H1B = Convert.ToInt16(attrs[15]);
                            //    statsMajor.H2B = Convert.ToInt16(attrs[16]);
                            //    statsMajor.H3B = Convert.ToInt16(attrs[17]);
                            //    statsMajor.IBB = Convert.ToInt16(attrs[21]);
                            //    statsMajor.HB = Convert.ToInt16(attrs[22]);
                            //    statsMajor.SF = Convert.ToInt16(attrs[23]);
                            //    statsMajor.SH = Convert.ToInt16(attrs[24]);

                            //    //await _pitchingService.AddPitchingStatAsync(statsMajor);
                            //}

                            //if (statsAAA != null)
                            //    await _pitchingService.AddPitchingStatAsync(statsAAA);
                            //if (statsAA != null)
                            //    await _pitchingService.AddPitchingStatAsync(statsAA);
                        }
                        else
                        {
                            Console.WriteLine("Not found pitcher {0} {1}", attrs[0].ExtractName(), attrs[1].ExtractName());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed for {0}, {1}", attrs[0].ExtractName(), attrs[1].ExtractName());
                    }
                }
                file.Close();
            }
            Console.WriteLine("Pitching parse completed");
        }
    }
}
