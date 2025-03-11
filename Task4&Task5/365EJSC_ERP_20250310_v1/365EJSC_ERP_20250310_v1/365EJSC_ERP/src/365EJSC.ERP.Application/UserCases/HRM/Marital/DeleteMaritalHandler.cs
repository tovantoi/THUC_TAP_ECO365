using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.Marital
{
    public class DeleteMaritalHandler : IRequestHandler<DeleteMaritalRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Marital"/>>  /// </summary>
        private readonly IMaritalSqlRepository hrmMaritalSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalHandler"/>, inject needed dependency
        /// </summary>
        public DeleteMaritalHandler(ISqlUnitOfWork sqlUnitOfWork, IMaritalSqlRepository hrmMaritalSqlRepository)
        {
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.hrmMaritalSqlRepository = hrmMaritalSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="DeleteWebLocalRequests"/>, find existing <see cref="Marital"/> base on id provided in <see cref="DeleteWebLocalRequests"/>,
        /// delete founded <see cref="Marital"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteMaritalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteMaritalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            HrmMarital? marital = await hrmMaritalSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (marital is null) CustomException.ThrowNotFoundException(typeof(HrmMarital), MsgCode.ERR_MARITAL_ID_NOT_FOUND);
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked sample as Deleted state
                hrmMaritalSqlRepository.Remove(marital);

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
