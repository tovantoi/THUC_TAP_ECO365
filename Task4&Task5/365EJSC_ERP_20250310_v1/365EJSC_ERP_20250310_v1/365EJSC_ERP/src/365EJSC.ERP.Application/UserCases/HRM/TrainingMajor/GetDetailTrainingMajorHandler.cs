using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.Validators.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor
{
    /// <summary>
    /// Handler for <see cref="GetDetailTrainingMajorRequest"/>
    /// </summary>
    public class GetDetailTrainingMajorHandler : IRequestHandler<GetDetailTrainingMajorRequest, Result<Entities.TrainingMajor>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="TrainingMajor"/>
        /// </summary>
        private readonly ITrainingMajorSqlRepository trainingMajorRepository;


        /// <summary>
        /// Constructor of <see cref="GetDetailTrainingMajorHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailTrainingMajorHandler(ITrainingMajorSqlRepository trainingMajorRepository)
        {
            this.trainingMajorRepository = trainingMajorRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailTrainingMajorRequest"/>, get <see cref="TrainingMajor"/> from database with id provided in <see cref="GetDetailTrainingMajorRequest"/>.
        /// Throw not found exception when <see cref="TrainingMajor"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="TrainingMajor"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<Entities.TrainingMajor>> Handle(GetDetailTrainingMajorRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            GetDetailTrainingMajorValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find training major by id provided. If training major not found will throw NotFoundException
            return await trainingMajorRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
        }
    }
}
