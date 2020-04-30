using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReadMLB.DataLayer.Helper;

namespace ReadMLB.DataLayer.Repositories
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public RepositoryAsync(DbContext context)
        {
            Context = context;
        }

        public virtual ValueTask<TEntity> GetAsync(object id) 
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.

            return Context.Set<TEntity>().FindAsync(id);
        }

        public virtual ValueTask<TEntity> GetAsync(params object[] keys)
        {
            return Context.Set<TEntity>().FindAsync(keys);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I didn't change it because I wanted the code to look like the videos. But feel free to change
            // this on your own.
            return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0)
        {
            var findResult = Context.Set<TEntity>().AsNoTracking().Where(predicate);
            if (orderBy != null)
                findResult = findResult.OrderBy(orderBy);
            if (take.HasValue)
                findResult = findResult.Take(take.Value);
            if (skip.HasValue)
                findResult = findResult.Skip(skip.Value);
            return await findResult.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TInc,TKey>(Expression<Func<TEntity,TInc>> include, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0)
        {
            var findResult = Context.Set<TEntity>().AsNoTracking().Include(include).Where(predicate);
            if (orderBy != null)
                findResult = findResult.OrderBy(orderBy);
            if (take.HasValue)
                findResult = findResult.Take(take.Value);
            if (skip.HasValue)
                findResult = findResult.Skip(skip.Value);
            return await findResult.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TKey>(IEnumerable<string> includes, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, int? take = 50, int? skip = 0)
        {
            var findResult = Context.Set<TEntity>().AsNoTracking();
            if (includes.Any())
            {
                //findResult = findResult.Include(includes.First()).;
                foreach (var include in includes)
                {
                    findResult = findResult.Include(include);
                }
            }
            
            findResult = findResult.Where(predicate);
            if (orderBy != null)
                findResult = findResult.OrderBy(orderBy);
            if (take.HasValue)
                findResult = findResult.Take(take.Value);
            if (skip.HasValue)
                findResult = findResult.Skip(skip.Value);
            return await findResult.ToListAsync();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return Context.Set<TEntity>().AddAsync(entity);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TEntity entity, IEnumerable<string> updatedProperties)
        {
            Context.Set<TEntity>().Attach(entity);
            foreach (var property in updatedProperties)
            {
                Context.Entry(entity).Property(property).IsModified = true;
            }
            return Task.CompletedTask;
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public Task RemoveAsync(int id)
        {
            var entity = Context.Set<TEntity>().Find(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity id {id} of type {typeof(TEntity).Name} not found. Unable to delete it.");
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

}
