using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using Grpc.Core;
using MediatR;
using ProtoResult;
using WardGrpcCommand;

namespace _365EJSC.ERP.Presentation.Services.Define.WebLocalWard
{
    public class WardCommandGrpcService : WardCommand.WardCommandBase
    {
        private readonly IMediator mediator;

        public WardCommandGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override async Task<CommonResult> CreateWard(CreateWardRequest request, ServerCallContext context)
        {
            try
            {
                CreateWardCommand? command = request.MapTo<CreateWardCommand>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        public override async Task<CommonResult> UpdateWard(UpdateWardRequest request, ServerCallContext context)
        {
            try
            {
                UpdateWardCommand? command = request.MapTo<UpdateWardCommand>();
                Result<object> result = await mediator.Send(command);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
        public override async Task<CommonResult> DeleteWard(DeleteWardRequest request, ServerCallContext context)
        {
            try
            {
                DeleteWardCommand? command = request.MapTo<DeleteWardCommand>();
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
