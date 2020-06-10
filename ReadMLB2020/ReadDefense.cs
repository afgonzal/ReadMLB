using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public class ReadDefense
    {
        private readonly IDefenseStatsService _defenseService;
        private readonly short _year;
        private readonly bool _inPO;
        private readonly string _defenseStats;
        private readonly FindPlayer _findPlayer;
        private readonly TeamsHelper _teamsHelper;
        public ReadDefense(IDefenseStatsService runningService, FindPlayer findPlayer, IConfiguration config, short year, bool inPO, TeamsHelper teamsHelper)
        {
            _defenseService = runningService;
            _year = year;
            _inPO = inPO;
            _teamsHelper = teamsHelper;
            _defenseStats = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}{config["BattingStats"]}");
            _findPlayer = findPlayer;
        }
        public async Task ReadDefenseAsync(IList<Player> players)
        {
            await _defenseService.CleanYearAsync(_year, _inPO);
            Console.WriteLine("Read Defense stats");
            using (var file = new StreamReader(_defenseStats))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var player = _findPlayer.FindPlayerById(players, Convert.ToInt64(attrs[1]), _year,
                        attrs[2].ExtractName(), attrs[3].ExtractName());
                    var defenseStat = new Defense
                    {
                        PlayerId = player.PlayerId,
                        TeamId = (attrs[4] == "-1") ? (byte)100 : _teamsHelper.GetActualTeam(Convert.ToByte(attrs[4]), Convert.ToByte(attrs[5])).TeamId,
                        League = Convert.ToByte(attrs[5]),
                        G = Convert.ToInt16(attrs[6]),
                        Year = _year,
                        InPO = _inPO,
                        PO = Convert.ToInt16(attrs[7]),
                        ASST = Convert.ToInt16(attrs[8]),
                        ERR = Convert.ToInt16(attrs[9])
                    };
                    await InsertToDbAsync(defenseStat);
                    //await _defenseService.AddDefenseStatsAsync(defenseStat);
                }
                file.Close();
            }
            Console.WriteLine("Defense read completed");
        }

        #region Batch Insert
        private readonly IList<Defense> _currentBatch = new List<Defense>();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private async Task InsertToDbAsync(Defense dstat)
        {
            await _semaphore.WaitAsync();
            _currentBatch.Add(dstat);
            if (_currentBatch.Count() >= 200)
            {

                await _defenseService.BatchInsertDefenseStatAsync(_currentBatch);
                Console.Write(".");
                _currentBatch.Clear();
            }
            _semaphore.Release();
        }

        private async Task FinishLastInsertBatch()
        {
            if (_currentBatch.Any())
            {
                await _defenseService.BatchInsertDefenseStatAsync(_currentBatch);
                _currentBatch.Clear();
            }
        }


        #endregion
    }
}