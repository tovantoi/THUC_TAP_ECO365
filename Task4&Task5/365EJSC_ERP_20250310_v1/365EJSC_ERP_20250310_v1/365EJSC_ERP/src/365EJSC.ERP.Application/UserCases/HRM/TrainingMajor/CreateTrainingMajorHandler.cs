using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.Validators.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor
{
    /// <summary>
    /// Handler for <see cref="CreateTrainingMajorRequest"/>
    /// </summary>
    public class CreateTrainingMajorHandler : IRequestHandler<CreateTrainingMajorRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="TrainingMajor"/>
        /// </summary>
        private readonly ITrainingMajorSqlRepository trainingMajorSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateTrainingMajorHandler"/>, inject needed dependency
        /// </summary>
        public CreateTrainingMajorHandler(ITrainingMajorSqlRepository trainingMajorSqlRepository,
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.trainingMajorSqlRepository = trainingMajorSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateTrainingMajorRequest"/>, create new <see cref="TrainingMajor"/> based on data <see cref="CreateTrainingMajorRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateTrainingMajorRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateTrainingMajorValidator validator = new();
            validator.ValidateAndThrow(request);


            // Create new TrainingMajor from request
            Entities.TrainingMajor? trainingMajor = request.MapTo<Entities.TrainingMajor>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked TrainingMajor as Created state
                trainingMajorSqlRepository.Add(trainingMajor);

                // Save data to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happened, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
