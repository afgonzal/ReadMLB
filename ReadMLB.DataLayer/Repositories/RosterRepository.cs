using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IRosterRepository : IRepositoryAsync<RosterPosition>
    {

    }
    public class RosterRepository : RepositoryAsync<RosterPosition>, IRosterRepository
    {
        public RosterRepository(DbContext context) : base(context)
        {

        }
    }
}
