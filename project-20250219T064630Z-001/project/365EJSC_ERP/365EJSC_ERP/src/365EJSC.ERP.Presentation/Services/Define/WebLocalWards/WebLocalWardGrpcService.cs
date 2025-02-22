using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Entities.Define;
using Grpc.Core;
using MediatR;
using ProtoResult;
using WardGrpc;

namespace _365EJSC.ERP.Presentation.Services.Define.WebLocalWards
{
    /// <summary>
    /// gRPC service for managing WebLocalWards, overrides from <see cref="WardService.WardServiceBase"/>
    /// </summary>
    public class WebLocalWardGrpcService : WardService.WardServiceBase
    {
        /// <summary>
        /// Mediator to handle requests
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="WebLocalWardGrpcService"/>, injects required dependencies
        /// </summary>
        /// <param name="mediator">Mediator instance</param>
        public WebLocalWardGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create a new <see cref="WebLocalWard"/> using mediator to send <see cref="CreateWebLocalWardRequest"/> to the handler
        /// </summary>
        /// <param name="request">Request to create a new <see cref="WebLocalWard"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> containing the result</returns>
        public override async Task<CommonResult> CreateWard(CreateWardRequest request, ServerCallContext context)
        {
            try
            {
                CreateWebLocalWardRequest? command = request.MapTo<CreateWebLocalWardRequest>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Update an existing <see cref="WebLocalWard"/> using mediator to send <see cref="UpdateWebLocalWardRequest"/> to the handler
        /// </summary>
        /// <param name="request">Request to update an existing <see cref="WebLocalWard"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> containing the result</returns>
        public override async Task<CommonResult> UpdateWard(UpdateWardRequest request, ServerCallContext context)
        {
            try
            {
                UpdateWebLocalWardRequest? command = request.MapTo<UpdateWebLocalWardRequest>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Delete an existing <see cref="WebLocalWard"/> by ID using mediator to send <see cref="DeleteWebLocalWardRequest"/> to the handler
        /// </summary>
        /// <param name="request">Request to delete an existing <see cref="WebLocalWard"/> by ID</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> containing the result</returns>
        public override async Task<CommonResult> DeleteWard(DeleteWardRequest request, ServerCallContext context)
        {
            try
            {
                DeleteWebLocalWardRequest? command = request.MapTo<DeleteWebLocalWardRequest>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Retrieve details of a specific <see cref="WebLocalWard"/> using mediator to send <see cref="GetDetailWebLocalWardRequest"/> to the handler
        /// </summary>
        /// <param name="request">Request to get details of a specific <see cref="WebLocalWard"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> containing the result</returns>
        public override async Task<CommonResult> GetWard(GetWardRequest request, ServerCallContext context)
        {
            try
            {
                GetDetailWebLocalWardRequest? query = request.MapTo<GetDetailWebLocalWardRequest>();
                Result<WebLocalWard> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Retrieve a list of all <see cref="WebLocalWard"/> using mediator to send <see cref="GetAllWebLocalWardRequest"/> to the handler
        /// </summary>
        /// <param name="request">Request to get all <see cref="WebLocalWard"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> containing the result</returns>
        public override async Task<CommonResult> GetAllWards(GetAllWardsRequest request, ServerCallContext context)
        {
            try
            {
                GetAllWebLocalWardRequest query = new();
                Result<List<WebLocalWard>> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
    }
}
