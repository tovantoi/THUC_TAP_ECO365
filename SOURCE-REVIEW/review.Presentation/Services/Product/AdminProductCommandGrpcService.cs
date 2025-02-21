using Grpc.Core;
using MediatR;
using ProtoResult;
using review.application.Requests.Product;
using review.Contract.Shared;
using AdminProduct;
using review.Contract.DependencyInjection.Extensions;
namespace review.Presentation.Services.Product
{
    /// <summary>
    /// Product grpc service override from <see cref="AdminProductCommand.AdminProductCommandBase"/>
    /// </summary>
    public class AdminProductCommandGrpcService : AdminProductCommand.AdminProductCommandBase
    {
        /// <summary>
        /// Mediator to send commands
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="AdminProductCommandGrpcService"/>, inject needed dependency
        /// </summary>
        public AdminProductCommandGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create new <see cref="Product"/>, use mediator send <see cref="CreateProductCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to create new <see cref="Product"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            try
            {
                CreateProductCommand? command = request.MapTo<CreateProductCommand>();
                //command.DueDate = request.DueDate?.ToDateTime();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Update existed <see cref="Product"/>, use mediator send <see cref="UpdateProductCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to update existed <see cref="Product"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            try
            {
                UpdateProductCommand? command = request.MapTo<UpdateProductCommand>();
               // command!.DueDate = request.DueDate?.ToDateTime();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Delete existed <see cref="Product"/> base on id, use mediator send <see cref="DeleteProductCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to delete existed <see cref="Product"/> by id</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            try
            {
                DeleteProductCommand? command = request.MapTo<DeleteProductCommand>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
    }
}