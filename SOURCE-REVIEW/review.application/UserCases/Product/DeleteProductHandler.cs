using MediatR;
using review.application.Requests.Product;
using review.application.Validators.Product;
using review.Contract.Shared;
using review.Domain.Abstractions.Repositories.Sql;
using review.Domain.Abstractions.Repositories.Sql.Base;
using System.Data;

namespace review.application.UserCases.Products
{
    /// <summary>
    /// Handler for <see cref="DeleteProductCommand"/>
    /// </summary>
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>>
        /// </summary>
        private readonly IProductSqlRepository productSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteProductHandler"/>, inject needed dependency
        /// </summary>
        public DeleteProductHandler(IProductSqlRepository productSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productSqlRepository = productSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteProductCommand"/>, find existing <see cref="Sample"/> base on id provided in <see cref="DeleteProductCommand"/>,
        /// delete founded <see cref="Sample"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteProductValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            Domain.Entities.Product sample = await productSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked sample as Deleted state
                productSqlRepository.Remove(sample);

                // Save changes to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happened, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}