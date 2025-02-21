using AdminProduct;
using Grpc.Core;
using MediatR;
using ProtoResult;
using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Shared;

namespace review.Presentation.Services.Product
{
    /// <summary>
    /// Product grpc service override from <see cref="AdminProductQuery.AdminProductQueryBase"/>
    /// </summary>
    public class AdminProductQueryGrpcService : AdminProductQuery.AdminProductQueryBase
    {
        /// <summary>
        /// Mediator to send queries
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="UserProductQueryGrpcService"/>, inject needed dependency
        /// </summary>
        public AdminProductQueryGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get existed <see cref="request"/> base on id, use mediator send <see cref="Product"/> to handler
        /// </summary>
        /// <param name="context">Request to get existed <see cref="CommonResult"/> by id</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="GetDetailProductQuery"/> that contain result</returns>
        public override async Task<CommonResult> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            try
            {
                GetDetailProductQuery? query = request.MapTo<GetDetailProductQuery>();
                Result<Domain.Entities.Product> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Get all <see cref="Product"/>, can skip specific number of records and limit records taken, use mediator send <see cref="GetAllProductQuery"/> to handler
        /// </summary>
        /// <param name="request">Request to get all <see cref="Product"/>, can skip specific number of records and limit records taken</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> GetAllProducts(GetAllProductsRequest request, ServerCallContext context)
        {
            try
            {
                GetAllProductsQuery query = new();
                Result<List<Domain.Entities.Product>> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
    }
}