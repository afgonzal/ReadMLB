using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public class ReadRunning
    {
        private readonly IRunningStatsService _runningService;
        private readonly short _year;
        private readonly bool _inPO;
        private readonly string _runningStats;
        private readonly FindPlayer _findPlayer;
        private readonly TeamsHelper _teamsHelper;
        public ReadRunning(IRunningStatsService runningService, FindPlayer findPlayer, IConfiguration config, short year, bool inPO, TeamsHelper teamsHelper)
        {
            _runningService = runningService;
            _year = year;
            _inPO = inPO;
            _teamsHelper = teamsHelper;
            _runningStats = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}{config["BattingStats"]}");
            _findPlayer = findPlayer;
        }
        public async Task ReadRunningAsync(IList<Player> players)
        {
            await _runningService.CleanYearAsync(_year, _inPO);
            Console.WriteLine("Read Running stats");
            using (var file = new StreamReader(_runningStats))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var player = _findPlayer.FindPlayerById(players, Convert.ToInt64(attrs[1]), _year,
                        attrs[2].ExtractName(), attrs[3].ExtractName());
                    var runningStat = new Running
                    {
                        PlayerId = player.PlayerId,
                        TeamId = (attrs[4] == "-1") ? (byte)100 : _teamsHelper.GetActualTeam(Convert.ToByte(attrs[4]), Convert.ToByte(attrs[5])).TeamId,
                        League = Convert.ToByte(attrs[5]),
                        Year = _year,
                        InPO = _inPO,
                        SB = Convert.ToInt16(attrs[10]),
                        CS = Convert.ToInt16(attrs[11]),
                        RS = Convert.ToInt16(attrs[12])
                    };

                    await _runningService.AddRunningStat(runningStat);
                }
                file.Close();
            }
            Console.WriteLine("Running status Completed");
        }
    }
}