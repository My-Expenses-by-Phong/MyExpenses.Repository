using MyExpenses.Domain.Core;

namespace MyExpenses.Repository
{
    public partial class EntityRepository<TEntity> where TEntity : BaseEntity
    {
        #region Private Utilities
        protected IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query,
            in bool includeDeleted = false)
        {
            if (includeDeleted) return query;

            return query.Where(e => !e.IsDeleted);
        }
        #endregion
    }
}
