using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private readonly IPlayersService _playserService;

        public ReadPlayers(IConfiguration config, IPlayersService playersService)
        {
            _playersSource = Path.Combine(config["SourceFolder"], config["SourceFile"]);
            _playersTemp = Path.Combine(config["SourceFolder"], config["PlayersTempList"]);
            _playserService = playersService;
        }

        public void ParsePlayers()
        {
            Console.WriteLine("Reading Players");
            ReadHelper.ReadList(_playersSource, "\"Player List\"", 1, 2, 3, false, _playersTemp);
            Console.WriteLine("Parse players complete.");
        }

        public IList<Player> GetPlayers()
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

        public async Task AddNewPlayersToDBAsync()
        {
            Console.WriteLine("Adding players to DB");
            var players = GetPlayers();
            int countNewPlayers = 0;
                foreach (var player in players)
                {

                    var result = await _playserService.AddAsync(player);
                    if (result >= 0)
                        countNewPlayers++;
                }
            Console.WriteLine("Players added to DB: {0}", countNewPlayers);
        }
    }
}