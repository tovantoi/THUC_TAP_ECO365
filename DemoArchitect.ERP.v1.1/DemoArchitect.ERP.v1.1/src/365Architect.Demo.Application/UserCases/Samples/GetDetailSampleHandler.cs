using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Entities;
using MediatR;

namespace _365Architect.Demo.Application.UserCases.Samples
{
    /// <summary>
    /// Handler for <see cref="GetDetailSampleQuery"/>
    /// </summary>
    public class GetDetailSampleHandler : IRequestHandler<GetDetailSampleQuery, Result<Sample>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>>
        /// </summary>
        private readonly ISampleSqlRepository sampleRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailSampleHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailSampleHandler(ISampleSqlRepository sampleRepository)
        {
            this.sampleRepository = sampleRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailSampleQuery"/>, get <see cref="Sample"/> from database with id provided in <see cref="GetDetailSampleQuery"/>.
        /// Throw not found exception when <see cref="Sample"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Sample"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Sample>> Handle(GetDetailSampleQuery request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailSampleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample by id provided. If sample not found will throw NotFoundException
            return await sampleRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
        }
    }
}