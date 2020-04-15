using Microsoft.Extensions.Configuration;
using System;
using ReadMLB.Services;
using System.Threading.Tasks;
using System.Threading;

namespace ReadMLB2020
{
    public class ReadMLBApp
    {
        private readonly IConfiguration _configuration;
        private readonly ITeamsService _teamsService;
        private readonly IPlayersService _playersService;
        private readonly IBattingService _battingService;
        private readonly bool _updateFiles;

        public ReadMLBApp(IConfiguration configuration, ITeamsService teamsService, IPlayersService playersService, IBattingService battingService)
        {
            _configuration = configuration;
            _teamsService = teamsService;
            _playersService = playersService;
            _battingService = battingService;
            _updateFiles = Convert.ToBoolean(_configuration["UpdateFiles"]);
        }
        public async Task RunAsync(string[] args)
        {
            Console.WriteLine("ReadMLB 2020 - Alejandro González intuido@yahoo.com");
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
            Task addPlayers;
            if (Convert.ToBoolean(_configuration["UpdatePlayers"]))
            {
                addPlayers = Task.Run(() => rp.AddNewPlayersToDBAsync());
            }
            else
            {
                addPlayers = Task.CompletedTask;
            }
            

            
            var rb = new ReadBatting(_battingService, _configuration, year);
            if (_updateFiles)
                rb.ParseBatting();
            Console.WriteLine("Update Batting stats");
            Task updateBatting;
            if (Convert.ToBoolean(_configuration["UpdateBatting"]))
            {
                updateBatting = Task.Run(() => rb.UpdateBattingStatsAsync(rp.GetPlayers()));
            }
            else
            {
                updateBatting = Task.CompletedTask;
            }

            await Task.WhenAll(addPlayers, updateBatting);
          
        }
    }
   
}
