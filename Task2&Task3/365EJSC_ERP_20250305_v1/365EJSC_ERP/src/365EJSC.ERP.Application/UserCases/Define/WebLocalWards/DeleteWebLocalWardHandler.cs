using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    /// <summary>
    /// Handler for <see cref="DeleteWebLocalWardRequest"/>
    /// </summary>
    public class DeleteWebLocalWardHandler : IRequestHandler<DeleteWebLocalWardRequest, Result<object>>
    {
        /// <summary>
        /// Repository handling data access of <see cref="WebLocalWard"/>
        /// </summary>
        private readonly IWebLocalWardSqlRepository wardsqlRepository;

        /// <summary>
        /// Unit of work to handle transactions
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public DeleteWebLocalWardHandler(IWebLocalWardSqlRepository wardsqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.wardsqlRepository = wardsqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteWebLocalWardRequest"/>, delete an existing <see cref="WebLocalWard"/>
        /// based on data in <see cref="DeleteWebLocalWardRequest"/> and save changes to the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(DeleteWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find ward base on id provided from database, if ward was not found, throw not found exception.
            // Need tracking to delete sample.
            WebLocalWard ward = await wardsqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked sample as Deleted state
                wardsqlRepository.Remove(ward);

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
