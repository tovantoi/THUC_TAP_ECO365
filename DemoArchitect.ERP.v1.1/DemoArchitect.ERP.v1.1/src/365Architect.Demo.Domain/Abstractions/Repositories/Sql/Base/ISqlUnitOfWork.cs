using System.Data;

namespace _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base
{
    /// <summary>
    /// Unit of work pattern to handle transaction
    /// </summary>
    public interface ISqlUnitOfWork : IDisposable
    {
        /// <summary>
        /// Apply all changes in context to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Database transaction, can be commited and rollback</returns>
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}