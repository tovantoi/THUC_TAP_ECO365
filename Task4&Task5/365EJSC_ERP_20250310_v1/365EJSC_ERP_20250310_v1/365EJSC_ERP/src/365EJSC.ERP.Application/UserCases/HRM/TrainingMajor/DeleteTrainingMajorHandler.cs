using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.Validators.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor
{
    /// <summary>
    /// Handler for <see cref="DeleteTrainingMajorRequest"/>
    /// </summary>
    public class DeleteTrainingMajorHandler : IRequestHandler<DeleteTrainingMajorRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="TrainingMajor"/>
        /// </summary>
        private readonly ITrainingMajorSqlRepository TrainingMajorSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteTrainingMajorHandler"/>, inject needed dependency
        /// </summary>
        public DeleteTrainingMajorHandler(ITrainingMajorSqlRepository TrainingMajorSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.TrainingMajorSqlRepository = TrainingMajorSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteTrainingMajorRequest"/>, find existing <see cref="TrainingMajor"/> based on id provided in <see cref="DeleteTrainingMajorRequest"/>,
        /// delete the found <see cref="TrainingMajor"/> and save to the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteTrainingMajorRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteTrainingMajorValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Training Major based on id provided from database, if Training Major was not found, throw not found exception.
            // Need tracking to delete Training Major.
            Entities.TrainingMajor trainingMajor = await TrainingMajorSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);



            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                TrainingMajorSqlRepository.Remove(trainingMajor);

                // Save changes to database
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
