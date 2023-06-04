namespace Shortening.API.UnitOfWorks.Abstracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
