using Microsoft.EntityFrameworkCore;
using MyExpenses.Domain;
using MyExpenses.Domain.Core;
using MyExpenses.Repository.Core;
using System.Linq.Expressions;

namespace MyExpenses.Repository
{
    public partial class EntityRepository<TEntity>(MyExpenseDbContext dbContext) : BaseRepository<TEntity>(dbContext)
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="isSoftDeleted">Whether to perform soft delete (mark as deleted) or hard delete.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public override async Task Delete(TEntity entity,
            bool isSoftDeleted = true,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (!isSoftDeleted)
            {
                Table.Remove(entity);
                return;
            }

            if (entity.IsDeleted) return;

            entity.IsDeleted = true;
            Table.Update(entity);
        }

        /// <summary>
        /// Deletes an entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to identify the entity to delete.</param>
        /// <param name="isSoftDeleted">Whether to perform soft delete (mark as deleted) or hard delete.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public override async Task Delete(Expression<Func<TEntity, bool>> predicate, bool isSoftDeleted = true,
            CancellationToken cancellationToken = default)
        {
            TEntity? entity = await Table.SingleOrDefaultAsync(predicate, cancellationToken);
            if (entity is null) return;

            await Delete(entity, isSoftDeleted, cancellationToken);
        }

        /// <summary>
        /// Gets a single entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to filter entities.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public override async Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default)
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            TEntity? entity = await query.SingleOrDefaultAsync(predicate, cancellationToken);

            return entity;
        }

        /// <summary>
        /// Gets an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public override async Task<TEntity?> Get(Guid id,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default)
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

            return entity;
        }

        /// <summary>
        /// Gets entities using a custom query function for advanced filtering and ordering.
        /// </summary>
        /// <param name="func">Function to apply custom query logic to the entity set.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of entities matching the query.</returns>
        public override async Task<IEnumerable<TEntity>> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> func,
            bool includeDeleted = false,
            CancellationToken cancellationToken = default)
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func(query);

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of all entities.</returns>
        public override async Task<IEnumerable<TEntity>> Get(bool includeDeleted = false,
            CancellationToken cancellationToken = default)
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>The inserted entity.</returns>
        public override async Task<TEntity> Insert(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await Table.AddAsync(entity, cancellationToken);

            return entity;
        }

        /// <summary>
        /// Inserts multiple entities into the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to insert.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>Collection of inserted entities.</returns>
        public override async Task<IEnumerable<TEntity>> Insert(IList<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);
            if (!entities.Any()) return [];

            await Table.AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity.</returns>
        public override async Task<TEntity> Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            Table.Update(entity);

            return entity;
        }
    }
}
