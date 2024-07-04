using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.UnitOfWorks
{
    public class UnitOfWork(DatabaseContext databaseContext): IUnitOfWork
    {
        public DatabaseContext DatabaseContext => databaseContext;


        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateTransactionAsync()
        {
            await databaseContext.Database.BeginTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await databaseContext.Database.RollbackTransactionAsync();
        }

    }
}
