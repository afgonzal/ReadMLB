using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IBattingRepository : IRepositoryAsync<Batting>
    {

    }
    public class BattingRepository : RepositoryAsync<Batting>, IBattingRepository
    {
        public BattingRepository(DbContext context) : base(context)
        {

        }
    }
}
