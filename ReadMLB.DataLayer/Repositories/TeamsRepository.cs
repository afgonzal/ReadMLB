using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface ITeamsRepository : IRepositoryAsync<Team>
    {
    }
    public class TeamsRepository : RepositoryAsync<Team>, ITeamsRepository
    {
        public TeamsRepository(DbContext context) : base(context)
        {

        }
    }
}

