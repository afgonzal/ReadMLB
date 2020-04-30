using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IRepositoryAsync<TEntity> : IDisposable where TEntity : class
    {
        ValueTask<TEntity> GetAsync(object id);

        ValueTask<TEntity> GetAsync(params object[] keys);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0);
        Task<IEnumerable<TEntity>> FindAsync<TInc, TKey>(Expression<Func<TEntity, TInc>> include, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0);
        Task<IEnumerable<TEntity>> FindAsync<TKey>(IEnumerable<string> includes, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, IEnumerable<string> updatedProperties);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(int id);

        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    }
}
