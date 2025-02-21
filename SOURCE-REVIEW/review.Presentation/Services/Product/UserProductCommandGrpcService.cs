using Grpc.Core;
using MediatR;
using ProtoResult;
using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Shared;
using UserProduct;

namespace review.Presentation.Services.Product
{
    /// <summary>
    /// Product grpc service override from <see cref="UserProductCommand.UserProductCommandBase"/>
    /// </summary>
    public class UserProductCommandGrpcService : UserProductCommand.UserProductCommandBase
    {
        /// <summary>
        /// Mediator to send commands
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="AdminProductCommandGrpcService"/>, inject needed dependency
        /// </summary>
        public UserProductCommandGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create new <see cref="request"/>, use mediator send <see cref="Product"/> to handler
        /// </summary>
        /// <param name="context">Request to create new <see cref="CommonResult"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CreateProductCommand"/> that contain result</returns>
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