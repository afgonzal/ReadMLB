using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IPlayersRepository : IRepositoryAsync<Player>
    {

    }
    public class PlayersRepository : RepositoryAsync<Player>, IPlayersRepository
    {
        public PlayersRepository(DbContext context) : base(context)
        {

        }
    }
}