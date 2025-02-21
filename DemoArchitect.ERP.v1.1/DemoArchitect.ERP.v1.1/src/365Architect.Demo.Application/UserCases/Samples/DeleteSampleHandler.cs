using System.Data;
using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;
using MediatR;

namespace _365Architect.Demo.Application.UserCases.Samples
{
    /// <summary>
    /// Handler for <see cref="DeleteSampleCommand"/>
    /// </summary>
    public class DeleteSampleHandler : IRequestHandler<DeleteSampleCommand, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>>
        /// </summary>
        private readonly ISampleSqlRepository sampleSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteSampleHandler"/>, inject needed dependency
        /// </summary>
        public DeleteSampleHandler(ISampleSqlRepository sampleSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.sampleSqlRepository = sampleSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteSampleCommand"/>, find existing <see cref="Sample"/> base on id provided in <see cref="DeleteSampleCommand"/>,
        /// delete founded <see cref="Sample"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteSampleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            Sample sample = await sampleSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked sample as Deleted state
                sampleSqlRepository.Remove(sample);

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