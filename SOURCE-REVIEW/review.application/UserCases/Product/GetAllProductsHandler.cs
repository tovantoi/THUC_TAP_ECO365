using MediatR;
using review.application.Requests.Product;
using review.Contract.Shared;
using review.Domain.Abstractions.Repositories.Sql;

namespace review.application.UserCases.Product
{
    /// <summary>
    /// Handler for <see cref="GetAllProductsQuery"/>
    /// </summary>
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<List<Domain.Entities.Product>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Product"/>>
        /// </summary>
        private readonly IProductSqlRepository productRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllProductsHandler"/>, inject needed dependency
        /// </summary>
        public GetAllProductsHandler(IProductSqlRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllProductsQuery"/>, get all Products in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="Product"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Domain.Entities.Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Domain.Entities.Product>>>(productRepository.FindAll().ToList());
        }
    }
}