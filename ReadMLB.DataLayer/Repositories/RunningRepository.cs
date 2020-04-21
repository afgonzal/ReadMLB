using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{

    public interface IRunningRepository : IRepositoryAsync<Running>
    {

    }
    public class RunningRepository : RepositoryAsync<Running>, IRunningRepository
    {
        public RunningRepository(DbContext context) : base(context)
        {

        }
    }
}