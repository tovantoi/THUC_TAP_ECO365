using System.Data;
using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;
using MediatR;

namespace _365Architect.Demo.Application.UserCases.Samples
{
    /// <summary>
    ///  Handler for <see cref="UpdateSampleCommand"/>/ 
    /// </summary>
    public class UpdateSampleHandler : IRequestHandler<UpdateSampleCommand, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Sample"/>> 
        /// </summary>
        private readonly ISampleSqlRepository sampleRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateSampleHandler"/>, inject needed dependency
        /// </summary>
        public UpdateSampleHandler(ISampleSqlRepository sampleRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.sampleRepository = sampleRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateSampleCommand"/>, find existing <see cref="Sample"/> base on id provided in <see cref="UpdateSampleCommand"/>,
        /// update founded <see cref="Sample"/> base on data provided in <see cref="UpdateSampleCommand"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateSampleCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateSampleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to update sample.
            Sample sample = await sampleRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Update sample base on data provided in UpdateSampleCommand request.
            // Keep sample original data if request fields is null
            request.MapTo(sample, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark sample as Updated state
                sampleRepository.Update(sample!);

                // Save sample to database
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