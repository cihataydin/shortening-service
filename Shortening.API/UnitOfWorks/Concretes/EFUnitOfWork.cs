using Microsoft.EntityFrameworkCore;
using Shortening.API.Contexts;
using Shortening.API.UnitOfWorks.Abstracts;

namespace Shortening.API.UnitOfWorks.Concretes
{
    public class EFUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _dbContext;
        public EFUnitOfWork(TContext dbContext)
        {
            if (dbContext == null)
                throw new Exception("database context is null");

            _dbContext = dbContext;
        }

        public async Task Dispose()
        {
            try
            {
                await _dbContext.DisposeAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
