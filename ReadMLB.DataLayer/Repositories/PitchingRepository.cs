using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IPitchingRepository : IRepositoryAsync<Pitching>
    {

    }
    public class PitchingRepository : RepositoryAsync<Pitching>, IPitchingRepository
    {
        public PitchingRepository(DbContext context) : base(context)
        {

        }
    }
}
