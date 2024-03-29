﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    internal class ReadPlayers
    {
        private readonly string _playersSource;
        private readonly string _playersTemp;
        private readonly IPlayersService _playersService;
        private readonly short _year;
        private IList<Player> _players;

        public ReadPlayers(IConfiguration config, IPlayersService playersService, short year, string sourceFile)
        {
            _playersSource = sourceFile;
            _playersTemp = Path.Combine(config["SourceFolder"], config["PlayersTempList"]);
            _playersService = playersService;
            _year = year;
        }

        public void ParsePlayers()
        {
            Console.WriteLine("Reading Players");
            ReadHelper.ReadList(_playersSource, "\"Player List\"", 1, 2, 3, false, _playersTemp);
            Console.WriteLine("Parse players complete.");
        }

        public async Task<IList<Player>> GetPlayersAsync()
        {
            return _players ?? (_players = (await _playersService.GetAll()).ToList());
        }
        public IList<Player> GetPlayersFromFile()
        {
            var players = new List<Player>();
            using (var file = new StreamReader(_playersTemp))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    players.Add(new Player { PlayerNumerator = Convert.ToInt32(attrs[0]), PlayerId = Convert.ToInt64(attrs[1]), FirstName = attrs[2].ExtractName(), LastName = attrs[3].ExtractName() });
                }
                file.Close();
            }
            return players;
        }

        public async Task AddNewPlayersToDbAsync()
        {
            Console.WriteLine("Adding players to DB");
            //await _playersService.CleanYearAsync(_year);
            var players = await GetPlayersAsync();
            int countNewPlayers = 0;
            using (var file = new StreamReader(_playersTemp))
            {
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    var attrs = line.Split(ReadHelper.Separator);
                    var newPlayer = new Player
                    {
                        PlayerNumerator = Convert.ToInt32(attrs[0]),
                        EAId = Convert.ToInt64(attrs[1]),
                        FirstName = attrs[2].ExtractName(),
                        LastName = attrs[3].ExtractName(),
                        Year = _year
                    };
                    var player = await _playersService.FindEAPlayerAsync(newPlayer.EAId, _year, newPlayer.FirstName, newPlayer.LastName);
                    
                    //Player player = null;
                    //throw new Exception("fix this part, <year or year-1 won-t work when you have multiple years");
                    //player = players.SingleOrDefault(p => p.EAId == newPlayer.EAId && p.Year < _year);
                    if (player == null || player.FirstName != newPlayer.FirstName || player.LastName != newPlayer.LastName)
                    {
                        var result = await _playersService.AddAsync(newPlayer);
                        if (result >= 0)
                            countNewPlayers++;
                    }
                }
                file.Close();
            }
            //reset cached players since we added some new ones
            if (countNewPlayers > 0)
                _players = null;
            Console.WriteLine("Players added to DB: {0}", countNewPlayers);
        }

        //public async Task VerifyPlayersAsync()
        //{
        //    var players = new List<Player>();
        //    using (var file = new StreamReader(_playersTemp))
        //    {
        //        string line;
        //        while ((line = file.ReadLine()) != null)
        //        {
        //            var attrs = line.Split(ReadHelper.Separator);
        //            var player = new Player { PlayerNumerator = Convert.ToInt32(attrs[0]), PlayerId = Convert.ToInt64(attrs[1]), FirstName = attrs[2].ExtractName(), LastName = attrs[3].ExtractName() };
        //            var dbPlayer = await _playersService.GetByIdAsync(player.PlayerId);
        //            if (dbPlayer != null)
        //            {
        //                dbPlayer.YearCreated = _year;
        //                await _playersService.UpdateAsync(dbPlayer);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Player not found {0}", player.PlayerId);
        //            }
        //        }
        //        file.Close();
        //    }
        //}
    }
}