namespace MyExpenses.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories
        #endregion

        Task<int> SaveChanges(CancellationToken cancellationToken = default);
    }
}
