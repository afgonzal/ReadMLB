using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
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

        public FindPlayer(IBattingService battingService)
        {
            _battingService = battingService;
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

            //Console.WriteLine("Duplicate player found {0} {1}", firstName, lastName);
            ////there are several with same name, what to do?
            Player validPlayer = null;
            
            foreach (var player in found)
            {
                var battingStats = await _battingService.GetPlayerBattingStatsAsync(player.PlayerId, year);
                if (teamId.HasValue && battingStats.Any(bs => bs.TeamId == teamId))
                {
                    return player;
                }

                if (battingStats.Any()) //found stats means the id is the valid one
                {
                    if (validPlayer == null)
                        validPlayer = player;
                    else //already found one!
                    {
                        Console.WriteLine("Multiple stats for player {0} {1}, {2} and {3}", firstName, lastName,
                            validPlayer.PlayerId, player.PlayerId);
                        return null;
                    }
                }
            }
            if (validPlayer != null)
            {
                return validPlayer;
            }
            //just take the first one
            return players.First();
            //Console.WriteLine("Not found valid data for player {0} {1}", firstName, lastName);
            //return null;
        }
    }
}
