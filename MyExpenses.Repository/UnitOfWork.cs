using MyExpenses.Domain;
using MyExpenses.Repository.Core;

namespace MyExpenses.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyExpenseDbContext _dbContext;

        public UnitOfWork(MyExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
