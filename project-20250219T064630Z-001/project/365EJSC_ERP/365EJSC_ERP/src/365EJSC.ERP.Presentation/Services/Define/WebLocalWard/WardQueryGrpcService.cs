using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Entities.Define;
using Grpc.Core;
using MediatR;
using ProtoResult;
using WardGrpcQuery;

namespace _365EJSC.ERP.Presentation.Services.Define.WebLocalWard
{
    public class WardQueryGrpcService : WardQuery.WardQueryBase
    {
        private readonly IMediator mediator;

        public WardQueryGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public override async Task<CommonResult> GetWard(GetWardRequest request, ServerCallContext context)
        {
            try
            {
                GetDetailWardQuery? query = request.MapTo<GetDetailWardQuery>();
                Result<WebsiteLocalizationWard> result = await mediator.Send(query);
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
                GetAllWardQuery query = new();
                Result<List<WebsiteLocalizationWard>> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
    }
}
