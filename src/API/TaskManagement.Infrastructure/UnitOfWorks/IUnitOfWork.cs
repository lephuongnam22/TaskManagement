using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        DatabaseContext DatabaseContext { get; }
        Task CreateTransactionAsync();
        Task CommitAsync(CancellationToken cancellationToken);
        Task RollbackAsync();
    }
}
