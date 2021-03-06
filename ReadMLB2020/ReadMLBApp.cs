﻿using Microsoft.Extensions.Configuration;
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
        private bool _inPO;
        private readonly IRunningStatsService _runningService;
        private readonly IDefenseStatsService _defenseService;
        private short _year;
        private readonly IRotationsService _rotationsService;
        private readonly IScheduleService _scheduleService;
        private readonly TeamsHelper _teamsHelper;

        public ReadMLBApp(IConfiguration configuration, ITeamsService teamsService, IPlayersService playersService, IBattingService battingService, IPitchingService pitchingService, FindPlayer findPlayer, IRostersService rostersService, IRunningStatsService runningService, IDefenseStatsService defenseService, 
            IRotationsService rotationsService, IScheduleService scheduleService)
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
            _defenseService = defenseService;
            _rotationsService = rotationsService;
            _scheduleService = scheduleService;
            _teamsHelper = new TeamsHelper(_teamsService);
        }
        public async Task RunAsync(string[] args)
        {
            Console.WriteLine("ReadMLB 2020 - Alejandro González intuido@yahoo.com");
           

            #region Read arguments and configuration
            
            var sourceFiles = _configuration["SourceFolder"];
            if (args.Length > 0)
            {
                _year = Convert.ToInt16(args[0]);
                if (_year < 2008 || _year > 2100)
                    throw new ArgumentException("Year must be btw 2008 and 2100");
            }
            else
            {
                _year = Convert.ToInt16(_configuration["Year"]);
            }

            if (args.Length > 1)
            {
                bool inPO;
                if (!bool.TryParse(args[1], out inPO))
                    throw new ArgumentException("InPO must be false or true.");
            }
            else
            {
                _inPO = Convert.ToBoolean(_configuration["InPO"]);
            }

            var sourceFile = Path.Combine(_configuration["SourceFolder"], $"{_year}{(_inPO ? 'P' : 'R')}.txt");
            if (!File.Exists(sourceFile))
                throw new ArgumentException("Source file not found.");

            var teamId = Convert.ToByte(_configuration["CurrentTeamId"]);
            if (teamId > 95)
                throw new ArgumentException($"Invalid current team {teamId}");
            var team = _teamsService.GetTeamByIdAsync(teamId).Result;
            #endregion
            Console.WriteLine("\n Source files {0}", sourceFiles);
            Console.WriteLine("Import franchise year {0}", _year);
            Console.WriteLine("Current team {0}", team.TeamName);

            if (Convert.ToBoolean(_configuration["RedirectToFile"]))
            {
                Console.SetOut(_outputWriter);
            }

            var rp = new ReadPlayers(_configuration, _playersService, _year, sourceFile);
            if (_updateFiles)
                rp.ParsePlayers();
            if (Convert.ToBoolean(_configuration["UpdatePlayers"]))
            {
                await rp.AddNewPlayersToDbAsync();
            }
            
            var rb = new ReadBatting(_battingService, _findPlayer, _configuration, _year,_inPO, sourceFile, _teamsHelper);
            if (_updateFiles)
                rb.ParseBatting();
            //Batting is required for all others
            if (Convert.ToBoolean(_configuration["UpdateBatting"]))
            {                
                 await rb.UpdateBattingStatsAsync(await rp.GetPlayersAsync());;
            }

            var rPitch = new ReadPitching(_pitchingService, _findPlayer, _configuration, _year, _inPO, sourceFile, _teamsHelper);
            if (_updateFiles)
                rPitch.ParsePitching();
            if (Convert.ToBoolean(_configuration["UpdatePitching"]))
            {
                var teams = _teamsHelper.Teams;
                await rPitch.UpdatePitchingStatsAsync(await rp.GetPlayersAsync(), teams.ToList());
            }

            //roster needs pitching to be ready
            var rRoster = new ReadRoster(_configuration, _year, _findPlayer, _rostersService, _rotationsService, _inPO, sourceFile);
            if (_updateFiles)
            {
                rRoster.ParseRoster();
                //rRoster.ParseRotation();
            }
            if (Convert.ToBoolean(_configuration["UpdateRosters"]))
            {
                //await rRoster.ValidatePlayersAsync(rp.GetPlayers());
                var teams = _teamsHelper.Teams;
                await rRoster.ReadRostersAsync(await rp.GetPlayersAsync(), teams.ToList());
            }
            if (Convert.ToBoolean(_configuration["UpdateRotations"]))
            {
                var teams = _teamsHelper.Teams;
                //await rRoster.ReadRotationsAsync(await rp.GetPlayersAsync(), teams);
                await rRoster.ReadPitcherAssignmentAsync(teams);
            }

           


            if (Convert.ToBoolean(_configuration["UpdateRunning"]))
            {
                var rRunning = new ReadRunning(_runningService,  _findPlayer, _configuration, _year, _inPO, _teamsHelper);
                await rRunning.ReadRunningAsync(await rp.GetPlayersAsync());
            }

            if (Convert.ToBoolean(_configuration["UpdateDefense"]))
            {
                var rDefense= new ReadDefense(_defenseService, _findPlayer, _configuration, _year, _inPO, _teamsHelper);
                await rDefense.ReadDefenseAsync(await rp.GetPlayersAsync());
            }

            if (Convert.ToBoolean(_configuration["UpdatePositions"]))
            {
                var rPositions = new ReadPlayerPositions(_playersService, _teamsService, _rostersService, _configuration, _year, _inPO);
                await rPositions.ParseRosterForPlayersAttrssAsync();
            }


            var rSchedule = new ReadSchedule(_teamsService, _scheduleService, _configuration, _year, _inPO, sourceFile);
            if (_updateFiles)
                rSchedule.ParseSchedule();
            if (Convert.ToBoolean(_configuration["UpdateSchedule"]))
            {
                await rSchedule.CleanScheduleAsync();
                var rScheduleAAA = new ReadSchedule(_teamsService, _scheduleService, _configuration, _year, _inPO,
                    sourceFile);
                var rScheduleAA = new ReadSchedule(_teamsService, _scheduleService, _configuration, _year, _inPO,
                    sourceFile);
                if (!_inPO)
                {
                    Task.WaitAll(new Task[]
                    {
                        rSchedule.UpdateScheduleAsync(0), rScheduleAAA.UpdateScheduleAsync(1),
                        rScheduleAA.UpdateScheduleAsync(2)
                    });

                }
                else
                {
                    Task.WaitAll(new Task[]
                    {
                        rSchedule.UpdatePlayoffsAsync(0), rScheduleAAA.UpdatePlayoffsAsync(1),
                        rScheduleAA.UpdatePlayoffsAsync(2)

                    });
                }

                await rSchedule.WriteToDbAsync();
                await rScheduleAAA.WriteToDbAsync();
                await rScheduleAA.WriteToDbAsync();
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
