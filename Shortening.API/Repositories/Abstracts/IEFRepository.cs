using Microsoft.EntityFrameworkCore;
using Shortening.API.Entities;
using System.Linq.Expressions;

namespace Shortening.API.Repositories.Abstracts
{
    public interface IEFRepository<TEntity, TContext> 
        where TEntity : BaseEntity 
        where TContext : DbContext
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity> CreateAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
