using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IPlayersRepository : IRepositoryAsync<Player>
    {
        Task<IEnumerable<Player>> SearchPlayers(byte? league, short? year, string firstNAme, string lastName, PlayerPositionAbr? position);
    }
    public class PlayersRepository : RepositoryAsync<Player>, IPlayersRepository
    {
        public PlayersRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Player>> SearchPlayers(byte? league, short? year, string firstNAme, string lastName, PlayerPositionAbr? position)
        {
            var query = Context.Set<RosterPosition>().Include(p => p.Team.Organization).
                Include(p => p.Player).AsNoTracking();
            //.Include(p => p.Player.Team.Organization)
            if (league.HasValue)
                query = query.Where(rp => rp.Team.League == league.Value);
            if (year.HasValue)
                query = query.Where(rp => rp.Year == year.Value);
            if (!string.IsNullOrEmpty(firstNAme))
                query = query.Where(rp => rp.Player.FirstName == firstNAme);
            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(rp => rp.Player.LastName == lastName);
            if (position.HasValue)
            {
                switch (position.Value)
                {
                    case PlayerPositionAbr.IF:
                        query = query.Where(rp =>
                            rp.Player.PrimaryPosition == PlayerPositionAbr.FB || rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.SB
                                                                              || rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.TB ||
                                                                              rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.SS
                                                                              || rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.IF
                                                                              || rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.FB ||
                                                                              rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.SB
                                                                              || rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.TB ||
                                                                              rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.SS
                                                                              || rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.IF);
                        break;
                    case PlayerPositionAbr.OF:
                        query = query.Where(rp =>
                            rp.Player.PrimaryPosition == PlayerPositionAbr.LF || rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.CF
                                                                              || rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.RF ||
                                                                              rp.Player.PrimaryPosition ==
                                                                              PlayerPositionAbr.OF
                                                                              || rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.LF ||
                                                                              rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.CF
                                                                              || rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.RF ||
                                                                              rp.Player.SecondaryPosition ==
                                                                              PlayerPositionAbr.OF);
                        break;
                    case PlayerPositionAbr.P:
                    case PlayerPositionAbr.RP:
                        query = query.Where(rp =>
                            rp.Player.PrimaryPosition == PlayerPositionAbr.RP || rp.Player.PrimaryPosition ==
                            PlayerPositionAbr.P || rp.Player.SecondaryPosition == PlayerPositionAbr.P || rp.Player.SecondaryPosition == PlayerPositionAbr.RP);
                        break;
                    case PlayerPositionAbr.C:
                    case PlayerPositionAbr.FB:
                    case PlayerPositionAbr.SB:
                    case PlayerPositionAbr.TB:
                    case PlayerPositionAbr.SS:
                    case PlayerPositionAbr.LF:
                    case PlayerPositionAbr.CF:
                    case PlayerPositionAbr.RF:
                        query = query.Where(rp =>
                            rp.Player.PrimaryPosition == position.Value || rp.Player.SecondaryPosition == position.Value);
                        break;
                }
            }
                

            var players = new List<Player>();
            foreach (var rosterPosition in await query.ToListAsync())
            {
                var player = rosterPosition.Player;
                if (players.All(p => p.PlayerId != player.PlayerId))
                {
                    player.Team = rosterPosition.Team;
                    players.Add(player);
                }
            }
            
            return players;
        }
    }
}