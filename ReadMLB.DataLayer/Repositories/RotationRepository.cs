using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IRotationRepository : IRepositoryAsync<RotationPosition>
    {

    }
    public class RotationRepository : RepositoryAsync<RotationPosition>, IRotationRepository
    {
        public RotationRepository(DbContext context) : base(context)
        {

        }
    }
}
