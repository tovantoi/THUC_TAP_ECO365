using MediatR;
using review.application.Requests.Product;
using review.application.Validators.Product;
using review.Contract.Shared;
using review.Domain.Abstractions.Repositories.Sql;

namespace review.application.UserCases.Product
{
    /// <summary>
    /// Handler for <see cref="GetDetailProductQuery"/>
    /// </summary>
    public class GetDetailProductHandler : IRequestHandler<GetDetailProductQuery, Result<Domain.Entities.Product>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>>
        /// </summary>
        private readonly IProductSqlRepository productRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailProductHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailProductHandler(IProductSqlRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailProductQuery"/>, get <see cref="Sample"/> from database with id provided in <see cref="GetDetailProductQuery"/>.
        /// Throw not found exception when <see cref="Sample"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Sample"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Domain.Entities.Product>> Handle(GetDetailProductQuery request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailProductValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample by id provided. If sample not found will throw NotFoundException
            return await productRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
        }
    }
}