using System.Data;
using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Shared;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;
using MediatR;

namespace _365Architect.Demo.Application.UserCases.Samples
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateSampleCommand"/>/ </summary>
    public class CreateSampleHandler : IRequestHandler<CreateSampleCommand, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Sample"/>>  /// </summary>
        private readonly ISampleSqlRepository sampleSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateSampleHandler"/>, inject needed dependency
        /// </summary>
        public CreateSampleHandler(ISampleSqlRepository sampleSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.sampleSqlRepository = sampleSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateSampleCommand"/>, create new <see cref="Sample"/> base on data <see cref="CreateSampleCommand"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateSampleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new sample from request
            Sample? sample = request.MapTo<Sample>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked sample as Created state
                sampleSqlRepository.Add(sample);

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