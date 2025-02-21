using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Entities;
using Grpc.Core;
using MediatR;
using ProtoResult;
using UserSample;

namespace _365Architect.Demo.Presentation.Services.Samples
{
    /// <summary>
    /// Sample grpc service override from <see cref="UserSampleQuery.UserSampleQueryBase"/>
    /// </summary>
    public class UserSampleQueryGrpcService : UserSampleQuery.UserSampleQueryBase
    {
        /// <summary>
        /// Mediator to send queries
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of <see cref="UserSampleQueryGrpcService"/>, inject needed dependency
        /// </summary>
        public UserSampleQueryGrpcService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get existed <see cref="Sample"/> base on id, use mediator send <see cref="GetDetailSampleQuery"/> to handler
        /// </summary>
        /// <param name="request">Request to get existed <see cref="Sample"/> by id</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> GetSample(GetSampleRequest request, ServerCallContext context)
        {
            try
            {
                GetDetailSampleQuery? query = request.MapTo<GetDetailSampleQuery>();
                Result<Sample> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }

        /// <summary>
        /// Get all <see cref="Sample"/>, can skip specific number of records and limit records taken, use mediator send <see cref="GetAllSampleQuery"/> to handler
        /// </summary>
        /// <param name="request">Request to get all <see cref="Sample"/>, can skip specific number of records and limit records taken</param>
        /// <param name="context">Current server call context</param>
        /// <returns><see cref="CommonResult"/> that contain result</returns>
        public override async Task<CommonResult> GetAllSamples(GetAllSamplesRequest request, ServerCallContext context)
        {
            try
            {
                GetAllSampleQuery query = new();
                Result<List<Sample>> result = await mediator.Send(query);
                return result.ConvertToCommonResult();
            }
            catch (Exception e)
            {
                return e.ConvertToCommonResult();
            }
        }
    }
}