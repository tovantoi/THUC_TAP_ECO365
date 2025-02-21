using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Entities;
using MediatR;

namespace _365Architect.Demo.Application.UserCases.Samples
{
    /// <summary>
    /// Handler for <see cref="GetAllSampleQuery"/>
    /// </summary>
    public class GetAllSampleHandler : IRequestHandler<GetAllSampleQuery, Result<List<Sample>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>>
        /// </summary>
        private readonly ISampleSqlRepository sampleRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllSampleHandler"/>, inject needed dependency
        /// </summary>
        public GetAllSampleHandler(ISampleSqlRepository sampleRepository)
        {
            this.sampleRepository = sampleRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllSampleQuery"/>, get all samples in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="Sample"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Sample>>> Handle(GetAllSampleQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Sample>>>(sampleRepository.FindAll().ToList());
        }
    }
}