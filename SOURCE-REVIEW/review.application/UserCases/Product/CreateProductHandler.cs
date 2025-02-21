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
    /// Hand/// Handler for <see cref="CreateProductCommand"/>/ </summary>
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Product"/>>  /// </summary>
        private readonly IProductSqlRepository productSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateProductHandler"/>, inject needed dependency
        /// </summary>
        public CreateProductHandler(IProductSqlRepository productSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productSqlRepository = productSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateProductCommand"/>, create new <see cref="Product"/> base on data <see cref="CreateProductCommand"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateProductValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new Product from request
            Domain.Entities.Product? product = request.MapTo<Domain.Entities.Product>();


            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Product as Created state
                productSqlRepository.Add(product);

                // Save data to database
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