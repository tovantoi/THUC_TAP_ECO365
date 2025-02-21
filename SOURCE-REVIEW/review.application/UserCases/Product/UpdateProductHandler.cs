using MediatR;
using review.application.Requests.Product;
using review.application.Validators.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Shared;
using review.Domain.Abstractions.Repositories.Sql;
using review.Domain.Abstractions.Repositories.Sql.Base;
using System.Data;

namespace review.application.UserCases.Product
{
    /// <summary>
    ///  Handler for <see cref="UpdateProductCommand"/>/ 
    /// </summary>
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="product"/>> 
        /// </summary>
        private readonly IProductSqlRepository productRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateProductHandler"/>, inject needed dependency
        /// </summary>
        public UpdateProductHandler(IProductSqlRepository productRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productRepository = productRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateProductCommand"/>, find existing <see cref="product"/> base on id provided in <see cref="UpdateProductCommand"/>,
        /// update founded <see cref="product"/> base on data provided in <see cref="UpdateProductCommand"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateProductValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find product base on id provided from database, if product was not found, throw not found exception.
            // Need tracking to update product.
            Domain.Entities.Product product = await productRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Update product base on data provided in UpdateproductCommand request.
            // Keep product original data if request fields is null
            request.MapTo(product, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark product as Updated state
                productRepository.Update(product!);

                // Save product to database
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