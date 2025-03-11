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
    /// Handler for <see cref="UpdateTrainingMajorRequest"/>
    /// </summary>
    public class UpdateTrainingMajorHandler : IRequestHandler<UpdateTrainingMajorRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="TrainingMajor"/>
        /// </summary>
        private readonly ITrainingMajorSqlRepository trainingMajorRepository;


        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateTrainingMajorHandler"/>, inject needed dependency
        /// </summary>
        public UpdateTrainingMajorHandler(ITrainingMajorSqlRepository trainingMajorRepository,
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.trainingMajorRepository = trainingMajorRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateTrainingMajorRequest"/>, find existing <see cref="TrainingMajor"/> based on id provided in <see cref="UpdateTrainingMajorRequest"/>,
        /// update the found <see cref="TrainingMajor"/> based on data provided in <see cref="UpdateTrainingMajorRequest"/> and save to the database.
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(UpdateTrainingMajorRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateTrainingMajorValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find TrainingMajor based on id provided from the database, if Training Major is not found, throw not found exception.
            // Need tracking to update the TrainingMajor.
            Entities.TrainingMajor TrainingMajor = await trainingMajorRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Update TrainingMajor based on data provided in UpdateTrainingMajorRequest request.
            // Keep TrainingMajor original data if request fields are null
            request.MapTo(TrainingMajor, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark TrainingMajor as Updated state
                trainingMajorRepository.Update(TrainingMajor!);

                // Save TrainingMajor to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happens, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
