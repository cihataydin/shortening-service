using Microsoft.EntityFrameworkCore;
using Shortening.API.Entities;
using Shortening.API.Repositories.Abstracts;
using System.Linq.Expressions;

namespace Shortening.API.Repositories.Concretes
{
    public class EFRepository<TEntity, TContext> : IEFRepository<TEntity, TContext>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbset;

        public EFRepository(TContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null) return _dbset.AsQueryable();

            return _dbset.Where(filter);
        }

        public async virtual Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _dbset.Update(entity);

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }
    }
}
