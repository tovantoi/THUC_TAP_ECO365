using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.Validators.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes
{
    public class DeleteContractTypeHandler : IRequestHandler<DeleteContractTypeRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="DefineContractType"/>>  /// </summary>
        private readonly IContractTypeSqlRepository contractTypeSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteContractTypeHandler"/>, inject needed dependency
        /// </summary>
        public DeleteContractTypeHandler(IContractTypeSqlRepository contractTypeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.contractTypeSqlRepository = contractTypeSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteContractTypeRequest"/>, find existing <see cref="DefineContractType"/> base on id contract type in <see cref="DeleteContractTypeRequest"/>,
        /// delete founded <see cref="DefineContractType"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteContractTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteContractTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find defineContractType base on id provided from database, if defineContractType was not found, throw not found exception.
            // Need tracking to delete defineContractType.
            DefineContractType defineContractType = await contractTypeSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            if(defineContractType is null)
            {
                CustomException.ThrowNotFoundException(typeof(DefineContractType), MsgCode.ERR_CONTRACT_TYPE_ID_NOT_FOUND);
            }

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked defineContractType as Deleted state
                contractTypeSqlRepository.Remove(defineContractType);

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
