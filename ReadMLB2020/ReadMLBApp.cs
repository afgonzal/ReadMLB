using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using ReadMLB.Services;
using System.Threading.Tasks;
using System.IO;

namespace ReadMLB2020
{
    public class ReadMLBApp
    {
        private readonly IConfiguration _configuration;
        private readonly ITeamsService _teamsService;
        private readonly IPlayersService _playersService;
        private readonly IBattingService _battingService;
        private readonly IPitchingService _pitchingService;
        private readonly IRostersService _rostersService;

        private readonly bool _updateFiles;
        private readonly FindPlayer _findPlayer;
        private StreamWriter _outputWriter;
        private FileStream _outputStream;
        private readonly bool _inPO;
        private readonly IRunningStatsService _runningService;

        public ReadMLBApp(IConfiguration configuration, ITeamsService teamsService, IPlayersService playersService, IBattingService battingService, IPitchingService pitchingService, FindPlayer findPlayer, IRostersService rostersService, IRunningStatsService runningService)
        {
            _configuration = configuration;
            _teamsService = teamsService;
            _playersService = playersService;
            _battingService = battingService;
            _pitchingService = pitchingService;
            _rostersService = rostersService;
            _updateFiles = Convert.ToBoolean(_configuration["UpdateFiles"]);
            _findPlayer = findPlayer;
            _outputStream = new FileStream(
                Path.Combine(_configuration["SourceFolder"], _configuration["ConsoleOutput"]), FileMode.Truncate,
                FileAccess.Write);
            _outputWriter = new StreamWriter(_outputStream);
            _inPO = Convert.ToBoolean(_configuration["inPO"]);
            _runningService = runningService;
        }
        public async Task RunAsync(string[] args)
        {
            Console.WriteLine("ReadMLB 2020 - Alejandro González intuido@yahoo.com");
            if (Convert.ToBoolean(_configuration["RedirectToFile"]))
            {
                Console.SetOut(_outputWriter);
            }

            #region Read arguments and configuration
            
            var sourceFiles = _configuration["SourceFolder"];            
            var year = Convert.ToInt16(args[0]);
            if (year < 2004 || year > 2100)
                throw new ArgumentException("Year must be btw 2004 and 2100");
            var teamId = Convert.ToByte(_configuration["CurrentTeamId"]);
            if (teamId < 0 || teamId > 95)
                throw new ArgumentException($"Invalid current team {teamId}");
            var team = _teamsService.GetTeamByIdAsync(teamId).Result;
            #endregion
            Console.WriteLine("\n Source files {0}", sourceFiles);
            Console.WriteLine("Import franchise year {0}", year);
            Console.WriteLine("Current team {0}", team.TeamName);

            
            var rp = new ReadPlayers(_configuration, _playersService);
            if (_updateFiles)
                rp.ParsePlayers();
            if (Convert.ToBoolean(_configuration["UpdatePlayers"]))
            {
                await rp.AddNewPlayersToDBAsync();
            }
            
            var rb = new ReadBatting(_battingService, _configuration, year,_inPO);
            if (_updateFiles)
                rb.ParseBatting();
            //Batting is required for all others
            if (Convert.ToBoolean(_configuration["UpdateBatting"]))
            {                
                 await rb.UpdateBattingStatsAsync(rp.GetPlayers());
            }


            var rRoster = new ReadRoster(_configuration, year, _findPlayer, _battingService, _rostersService, _inPO);
            if (_updateFiles)
                rRoster.ParseRoster();
            if (Convert.ToBoolean(_configuration["UpdateRosters"]))
            {
                //await rRoster.ValidatePlayersAsync(rp.GetPlayers());
                var teams = await _teamsService.GetTeamsAsync();
                await rRoster.ReadRostersAsync(rp.GetPlayers(), teams.ToList());
            }

            var rPitch = new ReadPitching(_pitchingService, _rostersService, _findPlayer, _configuration, year, _inPO);
            if (_updateFiles)
                rPitch.ParsePitching();
            if (Convert.ToBoolean(_configuration["UpdatePitching"]))
            {
                var teams = await _teamsService.GetTeamsAsync();
                await rPitch.UpdatePitchingStatsAsync(rp.GetPlayers(), teams.ToList());
            }


            if (Convert.ToBoolean(_configuration["UpdateRunning"]))
            {
                var rRunning = new ReadRunning(_runningService, _configuration, year, _inPO);
                await rRunning.ReadRunningAsync();
            }

            if (Convert.ToBoolean(_configuration["RedirectToFile"]))
            {
                _outputWriter.Close();
                _outputStream.Close();
                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }

            Console.WriteLine("ReadMLB done.");
        }
    }
}
