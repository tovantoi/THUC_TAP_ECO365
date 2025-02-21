using System.Linq.Expressions;
using _365Architect.Demo.Domain.Abstractions.Entities;

namespace _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base
{
    /// <summary>
    /// Provide generic repository
    /// </summary>
    /// <typeparam name="TEntity">Generic type of Domain entity</typeparam>
    /// <typeparam name="TKey">Generic key of Domain entity</typeparam>
    public interface IGenericSqlRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        /// <summary>
        /// Find entity by id. Returned entity can be tracking
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        Task<TEntity?> FindByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Find single entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity matched expression or null if entity not found</returns>
        Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate,
                                       bool isTracking = false,
                                       CancellationToken cancellationToken = default,
                                       params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Check entity with specific predicate is exist in current application
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if entity exist, otherwise false</returns>
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find all entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>IQueryable of entities that match predicate expression</returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, bool isTracking = false, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Marked entity as Added state
        /// </summary>
        /// <param name="entity">Added entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Marked entity as Updated state
        /// </summary>
        /// <param name="entity">Updated entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Marked entity as Deleted state
        /// </summary>
        /// <param name="entity">Removed entity</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Marked multiple entities as Deleted state
        /// </summary>
        /// <param name="entities">Removed entities</param>
        void RemoveMultiple(List<TEntity> entities);
    }
}