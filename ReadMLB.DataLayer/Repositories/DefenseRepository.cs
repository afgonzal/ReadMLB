using Microsoft.EntityFrameworkCore;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IDefenseRepository : IRepositoryAsync<Defense>
    {

    }
    public class DefenseRepository : RepositoryAsync<Defense>, IDefenseRepository
    {
        public DefenseRepository(DbContext context) : base(context)
        {

        }
    }
}
