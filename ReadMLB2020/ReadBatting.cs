using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        private readonly FindPlayer _findPlayer;
        private readonly TeamsHelper _teamsHelper;

        public ReadBatting(IBattingService battingService, FindPlayer findPlayer, IConfiguration config, short year, bool inPO, string sourceFile, TeamsHelper teamsHelper)
        {
            _inPO = inPO;
            _year = year;
            _battingSource = sourceFile;
            _teamsHelper = teamsHelper;
            _battingStats = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}{config["BattingStats"]}");
            _battingTemp = Path.Combine(config["SourceFolder"], config["BattingTempStats"]);
            _battingService = battingService;
            _findPlayer = findPlayer;
        }

        private List<Batting> ParseBattingTemp()
        {
            var stats = new List<Batting>();
            using (var file = new StreamReader(_battingTemp))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    stats.Add(new Batting
                    {
                        PA = Convert.ToInt16(attrs[15]),
                        H1B = Convert.ToInt16(attrs[16]),
                        H2B = Convert.ToInt16(attrs[17]),
                        H3B = Convert.ToInt16(attrs[18]),
                        HR = Convert.ToInt16(attrs[19]),
                        RBI = Convert.ToInt16(attrs[20]),
                        SO = Convert.ToInt16(attrs[21]),
                        BB = Convert.ToInt16(attrs[22]),
                        BattingVs = BattingVs.Left,
                        InPO = _inPO,
                        League = 0,
                        PlayerId = Convert.ToInt64(attrs[0]), //store enumerator temporarily
                        Year = _year
                    });
                    
                    stats.Add( new Batting
                    {
                        PA = Convert.ToInt16(attrs[23]),
                        H1B = Convert.ToInt16(attrs[24]),
                        H2B = Convert.ToInt16(attrs[25]),
                        H3B = Convert.ToInt16(attrs[26]),
                        HR = Convert.ToInt16(attrs[27]),
                        RBI = Convert.ToInt16(attrs[28]),
                        SO = Convert.ToInt16(attrs[29]),
                        BB = Convert.ToInt16(attrs[30]),
                        BattingVs = BattingVs.Right,
                        InPO = _inPO,
                        League = 0,
                        PlayerId = Convert.ToInt64(attrs[0]), //store enumerator temporarily,
                        Year = _year
                    });
                }
                file.Close();
            }

            return stats;
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
                        TeamId = (attrs[4] == "-1") ? (byte)100 : Convert.ToByte(attrs[4]),
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
                    });
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
        public async Task UpdateBattingStatsAsync(IList<Player> players)
        {
            Console.WriteLine("Update Batting stats");
            var timer = new Stopwatch();
            timer.Start();
            await _battingService.CleanYearAsync(_year, _inPO);
            var tempStats = ParseBattingTemp();
            var j = tempStats.Where(p => p.PlayerId == 6);
            //Iterate Battting stats
            using (var file = new StreamReader(_battingStats))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    try
                    {
                        var player = _findPlayer.FindPlayerById(players, Convert.ToInt64(attrs[1]), _year, attrs[2].ExtractName(), attrs[3].ExtractName());
                        var realTeam = (attrs[4] == "-1") ? null : _teamsHelper.GetActualTeam(Convert.ToByte(attrs[4]), Convert.ToByte(attrs[5]));
                        var bstat = new Batting
                        {
                            PlayerId = player.PlayerId,
                            TeamId = (attrs[4] == "-1") ? (byte) 100 : realTeam.TeamId,
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
                        };
                        //await _battingService.AddBattingStatAsync(bstat);
                        await InsertToDbAsync(bstat);

                        if (bstat.League == 0 && bstat.PA > 0)
                        {
                            //find if we have stats for same player enum
                            var splitStats = tempStats.Where(ts => ts.PlayerId == Convert.ToInt64(attrs[0]));
                            foreach (var tempStat in splitStats)
                            {
                                var bs = JsonConvert.DeserializeObject<Batting>(JsonConvert.SerializeObject(tempStat));
                                bs.G = bstat.G;
                                bs.PlayerId = bstat.PlayerId;
                                bs.TeamId = bstat.TeamId;
                               // await _battingService.AddBattingStatAsync(bs);
                               await InsertToDbAsync(bs);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed for {0}", attrs[1]);
                    }

                }
                file.Close();
            }
            //must complete last batch
            await FinishLastInsertBatch();
            timer.Stop();
            Console.WriteLine("\nBatting update complete. {0}s", timer.Elapsed.TotalSeconds);
        }

        

        #region Batch Insert
        private readonly IList<Batting> _currentBatch = new List<Batting>();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private async Task InsertToDbAsync(Batting bstat)
        {
            await _semaphore.WaitAsync();
            _currentBatch.Add(bstat);
            if (_currentBatch.Count() >= 200)
            {

                await _battingService.BatchInsertBattingStatAsync(_currentBatch);
                Console.Write(".");
                _currentBatch.Clear();
            }
            _semaphore.Release();
        }

        private async Task FinishLastInsertBatch()
        {
            if (_currentBatch.Any())
            {
                await _battingService.BatchInsertBattingStatAsync(_currentBatch);
                _currentBatch.Clear();
            }
        }

      
        #endregion
    }
   
}
