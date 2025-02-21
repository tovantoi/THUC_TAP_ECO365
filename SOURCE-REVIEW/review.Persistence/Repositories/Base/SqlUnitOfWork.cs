using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using review.Domain.Abstractions.Repositories.Sql.Base;

namespace review.Persistence.Repositories.Base
{
    /// <summary>
    /// Unit of work pattern to handle transaction
    /// </summary>
    public class SqlUnitOfWork : ISqlUnitOfWork
    {
        /// <summary>
        /// Database context to begin transaction
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Store began transaction
        /// </summary>
        private IDbContextTransaction transaction;

        public SqlUnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Apply all changes in context to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        /// <summary>
        /// Dispose current transaction
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            transaction?.Dispose();
        }
    }
}