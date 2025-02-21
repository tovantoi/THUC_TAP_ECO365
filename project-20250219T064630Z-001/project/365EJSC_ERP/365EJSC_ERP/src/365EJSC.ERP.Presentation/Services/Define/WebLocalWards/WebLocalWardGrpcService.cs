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
    public class WebLocalWardGrpcService : WardService.WardServiceBase
    {
        private readonly IMediator mediator;

        public WebLocalWardGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

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
