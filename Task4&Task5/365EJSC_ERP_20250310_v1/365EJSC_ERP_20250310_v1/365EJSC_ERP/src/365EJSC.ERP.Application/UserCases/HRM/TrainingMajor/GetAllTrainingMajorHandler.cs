using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor
{
    /// <summary>
    /// Handler for <see cref="GetAllTrainingMajorRequest"/>
    /// </summary>
    public class GetAllTrainingMajorHandler : IRequestHandler<GetAllTrainingMajorRequest, Result<List<Entities.TrainingMajor>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="TrainingMajor"/>
        /// </summary>
        private readonly ITrainingMajorSqlRepository trainingMajorSqlRepository;


        /// <summary>
        /// Constructor of <see cref="GetAllTrainingMajorHandler"/>, inject needed dependency
        /// </summary>
        public GetAllTrainingMajorHandler(ITrainingMajorSqlRepository trainingMajorSqlRepository)
        {
            this.trainingMajorSqlRepository = trainingMajorSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllTrainingMajorRequest"/>, get all train major in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="TrainingMajor"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Entities.TrainingMajor>>> Handle(GetAllTrainingMajorRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Entities.TrainingMajor>>>(trainingMajorSqlRepository.FindAll().ToList());
        }
    }
}
