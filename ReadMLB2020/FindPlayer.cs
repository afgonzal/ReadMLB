using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public static class PlayerExtensions
    {
        public static string ExtractName(this string value)
        {
            return value.Replace("\"", "").TrimEnd();
        }
    }
    public class FindPlayer
    {
        private readonly IBattingService _battingService;
        private readonly IPitchingService _pitchingService;

        public FindPlayer(IBattingService battingService, IPitchingService pitchingService)
        {
            _battingService = battingService;
            _pitchingService = pitchingService;
        }

        public async Task<Player> FindPlayerByName(IList<Player> players, string firstName, string lastName, short year, byte? teamId = null)
        {
            var found = players.Where(p =>
                p.FirstName == firstName &&
                p.LastName == lastName).ToList();
            if (!found.Any())
            {
                Console.WriteLine("Player {0}, {1} not found", firstName, lastName);
                throw new Exception("Player not found");
            }

            if (found.Count == 1)
                return found.First();

            ////there are several with same name, what to do?
            foreach (var player in found)
            {
                var battingStats = await _battingService.GetPlayerBattingStatsAsync(player.PlayerId, year);
                //stats for same team means he is same guy
                if (teamId.HasValue && battingStats.Any(bs => bs.TeamId == teamId))
                {
                    return player;
                }

                var pitchingStats = await _pitchingService.GetPlayerPitchingStatsAsync(player.PlayerId, year);
                //stats for same team means he is that guy
                if (teamId.HasValue && pitchingStats.Any(ps => ps.TeamId == teamId))
                {
                    return player;
                }
            }

            //just take the first one
            Console.WriteLine("Not found valid data for player {0} {1}", firstName, lastName);
            return players.First();
            
            //return null;
        }

        //public async Task<Player> FindPitcherByName(IList<Player> players, string firstName, string lastName,
        //    short year, byte teamId)
        //{
        //    var found = players.Where(p => p.FirstName == firstName && p.LastName == lastName).ToList();
        //    var pitchers = new List<Player>();
        //    foreach (var player in found)
        //    {
        //        var pStats = await _pitchingService.GetPlayerPitchingHistoryAsync(player.PlayerId);
        //        if (!pStats.Any())
        //            return null;
                

        //    }
        //}
        public async Task<Player> FindPitcherByName(IList<Player> players, string firstName, string lastName,
            short year)
        {
            if (year <= 2008) //doesn't work for 1st year
                return null;
            var found = players.Where(p =>
                p.FirstName == firstName &&
                p.LastName == lastName).ToList();
            //we assume pitch stats for these year are not ready, so look in previous years
            var pitchers = new List<Player>();
            foreach (var player in found)
            {
                var pStats = await _pitchingService.GetPlayerPitchingHistoryAsync(player.PlayerId);
                if (pStats.Any())
                    pitchers.Add(player);
            }

            if (pitchers.Count == 1)
                return pitchers.First();
            else if (pitchers.Count == 0)
                return null;
            Console.WriteLine("Several pitchers with same name {0} {1}", firstName, lastName);
            return null;
        }

        public Player FindPlayerById(IList<Player> players, long eaId, short year, string firstName, string lastName)
        {
            Player player;
            var foundPlayers = players.Where(p => p.EAId == eaId && p.FirstName == firstName && p.LastName == lastName).ToList();
            if (players.Count() == 1)
                player = players.First();
            else
            {
                //filter again by name
                foundPlayers = foundPlayers.Where(p => p.FirstName == firstName && p.LastName == lastName).ToList();
                if (foundPlayers.Count() == 1)
                    player = foundPlayers.First();
                else
                    player = foundPlayers.Single(p => p.Year == year);
            }

            return player;
        }

        public Player FindPlayerById(IList<Player> players, long eaId, short year)
        {
            var foundPlayers = players.Where(p => p.EAId == eaId).ToList();
            if (foundPlayers.Count() > 1)
            {
                //find the newest one, but consider that it has can't be younger than year   
                return foundPlayers.Where(p=> p.Year<= year).OrderByDescending(p => p.Year).FirstOrDefault();
            }
            //there's just one
            return foundPlayers.FirstOrDefault();
        }
    }
}
