using System.Linq.Expressions;
using _365Architect.Demo.Domain.Abstractions.Entities;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using Microsoft.EntityFrameworkCore;

namespace _365Architect.Demo.Persistence.Repositories.Base
{
    /// <summary>
    /// Implementation of IGenericRepository
    /// </summary>
    /// <typeparam name="TEntity">Generic type of Domain entity</typeparam>
    /// <typeparam name="TKey">Generic key of Domain entity</typeparam>
    public class GenericSqlRepository<TEntity, TKey> : IGenericSqlRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        /// <summary>
        /// Database context to interact with database
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Entities, as table in database
        /// </summary>
        private DbSet<TEntity>? entities;

        /// <summary>
        /// Constructor of <see cref="GenericSqlRepository{TEntity,TKey}"/>,  inject needed dependency
        /// </summary>
        /// <param name="context"></param>
        public GenericSqlRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get entity DbSet, if entities is null, use context to get db set of entities
        /// </summary>
        protected DbSet<TEntity> Entities => entities ??= context.Set<TEntity>();

        /// <summary>
        /// Find entity by id. Returned entity can be tracking
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<TEntity?> FindByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            IQueryable<TEntity> query = Entities.AsQueryable();

            if (includeProperties.Any())
                // Include specified properties
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();

            // Find entity by Id
            TEntity? entity = await query.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);

            // Return founded entity, null when entity was not found
            return entity;
        }

        /// <summary>
        /// Find single entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity matched expression or null if entity not found</returns>
        public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate,
                                                    bool isTracking = false,
                                                    CancellationToken cancellationToken = default,
                                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            IQueryable<TEntity> query = Entities.AsQueryable();

            if (includeProperties.Any())
                // Include specified properties
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();

            // Apply predicate if provided, otherwise return a single entity
            TEntity? entity = predicate is not null ? await query.FirstOrDefaultAsync(predicate, cancellationToken) : await query.FirstOrDefaultAsync(cancellationToken);

            // Return founded entity, null when entity was not found
            return entity;
        }

        /// <summary>
        /// Check entity with specific predicate is exist in database
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if entity exist, otherwise false</returns>
        public Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            // Initialize query from the entity set
            IQueryable<TEntity> query = Entities.AsQueryable();

            // Return true if entity with predicate exist in database, otherwise false
            return Task.FromResult(query.Where(predicate).Any());
        }

        /// <summary>
        /// Find all entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns><see cref="IQueryable{T}"/> of entities that match predicate expression</returns>
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, bool isTracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            IQueryable<TEntity> query = Entities.AsQueryable();

            if (includeProperties.Any())
                // Include specified properties
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();

            // Apply predicate if provided, otherwise return the query
            return predicate is not null ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Marked entity as Added state
        /// </summary>
        /// <param name="entity">Added entity</param>
        public void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Marked entity as Updated state
        /// </summary>
        /// <param name="entity">Updated entity</param>
        public void Update(TEntity entity)
        {
            Entities.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Marked entity as Deleted state
        /// </summary>
        /// <param name="entity">Removed entity</param>
        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// Marked multiple entities as Deleted state
        /// </summary>
        /// <param name="entitiesToRemove">Removed entities</param>
        public void RemoveMultiple(List<TEntity> entitiesToRemove)
        {
            Entities.RemoveRange(entitiesToRemove);
        }

        /// <summary>
        /// Extension method of <see cref="IQueryable{T}"/> for including multiple relationship
        /// </summary>
        /// <typeparam name="TEntity">Type of Domain entity</typeparam>
        /// <param name="source">IQueryable source need to including properties</param>
        /// <param name="includeProperties">Properties to be included</param>
        /// <returns><see cref="IQueryable{T}"/> with included properties</returns>
        private IQueryable<TEntity> IncludeMultiple(IQueryable<TEntity> source, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties.Any())
                // Each property will be included into source
                source = includeProperties.Aggregate(source, (current, include) => current.Include(include));
            return source;
        }
    }
}