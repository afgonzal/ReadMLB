using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ReadRunning(IRunningStatsService runningService, IConfiguration config, short year, bool inPo)
        {
            _runningService = runningService;
            _year = year;
            _inPO = inPo;
            _runningStats = Path.Combine(config["SourceFolder"], config["BattingStats"]);
        }
        public async Task ReadRunningAsync()
        {
            await _runningService.CleanYearAsync(_year, _inPO);
            Console.WriteLine("Read Running stats");
            using (var file = new StreamReader(_runningStats))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var runningStat = new Running
                    {
                        PlayerId = Convert.ToInt64(attrs[1]),
                        TeamId = (attrs[4] == "-1") ? (byte)100 : Convert.ToByte(attrs[4]),
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