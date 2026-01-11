using Microsoft.EntityFrameworkCore;
using MyExpenses.Domain;
using MyExpenses.Domain.Core;
using System.Linq.Expressions;

namespace MyExpenses.Repository.Core
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        #region Properties
        protected MyExpenseDbContext Context { get; }
        protected DbSet<TEntity> Table { get; }
        #endregion

        public BaseRepository(MyExpenseDbContext dbContext)
        {
            Context = dbContext;
            Table = dbContext.Set<TEntity>();
        }

        #region Methods
        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="isSoftDeleted">Whether to perform soft delete (mark as deleted) or hard delete.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public abstract Task Delete(TEntity entity,
            bool isSoftDeleted = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to identify the entity to delete.</param>
        /// <param name="isSoftDeleted">Whether to perform soft delete (mark as deleted) or hard delete.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public abstract Task Delete(Expression<Func<TEntity, bool>> predicate,
            bool isSoftDeleted = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to filter entities.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public abstract Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public abstract Task<TEntity?> Get(Guid id,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities using a custom query function for advanced filtering and ordering.
        /// </summary>
        /// <param name="func">Function to apply custom query logic to the entity set.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of entities matching the query.</returns>
        public abstract Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> func,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of all entities.</returns>
        public abstract Task<IEnumerable<TEntity>> Get(bool includeDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The inserted entity.</returns>
        public abstract Task<TEntity> Insert(TEntity entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts multiple entities into the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to insert.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of inserted entities.</returns>
        public abstract Task<IEnumerable<TEntity>> Insert(IList<TEntity> entities,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity.</returns>
        public abstract Task<TEntity> Update(TEntity entity);
        #endregion
    }
}
