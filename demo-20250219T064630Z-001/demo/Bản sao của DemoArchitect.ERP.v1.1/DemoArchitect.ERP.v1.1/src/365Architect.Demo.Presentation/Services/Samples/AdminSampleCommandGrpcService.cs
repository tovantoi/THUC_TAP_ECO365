using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Entities;
using AdminSample;
using Grpc.Core;
using MediatR;
using ProtoResult;

namespace _365Architect.Demo.Presentation.Services.Samples
{
    /// <summary>
    /// Sample grpc service override from <see cref="AdminSampleCommand.AdminSampleCommandBase"/>
    /// </summary>
    public class AdminSampleCommandGrpcService : AdminSampleCommand.AdminSampleCommandBase
    {
        /// <summary>
        /// Mediator to send commands
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="AdminSampleCommandGrpcService"/>, inject needed dependency
        /// </summary>
        public AdminSampleCommandGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create new <see cref="Sample"/>, use mediator send <see cref="CreateSampleCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to create new <see cref="Sample"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> CreateSample(CreateSampleRequest request, ServerCallContext context)
        {
            try
            {
                CreateSampleCommand? command = request.MapTo<CreateSampleCommand>();
                command.DueDate = request.DueDate?.ToDateTime();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Update existed <see cref="Sample"/>, use mediator send <see cref="UpdateSampleCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to update existed <see cref="Sample"/></param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> UpdateSample(UpdateSampleRequest request, ServerCallContext context)
        {
            try
            {
                UpdateSampleCommand? command = request.MapTo<UpdateSampleCommand>();
                command!.DueDate = request.DueDate?.ToDateTime();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Delete existed <see cref="Sample"/> base on id, use mediator send <see cref="DeleteSampleCommand"/> to handler
        /// </summary>
        /// <param name="request">Request to delete existed <see cref="Sample"/> by id</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> DeleteSample(DeleteSampleRequest request, ServerCallContext context)
        {
            try
            {
                DeleteSampleCommand? command = request.MapTo<DeleteSampleCommand>();
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