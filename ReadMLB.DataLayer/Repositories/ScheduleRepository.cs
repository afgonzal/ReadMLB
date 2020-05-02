using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IScheduleRepository : IRepositoryAsync<MatchResult>
    {

    }
    public class ScheduleRepository : RepositoryAsync<MatchResult>, IScheduleRepository
    {
        public ScheduleRepository(DbContext context) : base(context)
        {

        }
    }
}
