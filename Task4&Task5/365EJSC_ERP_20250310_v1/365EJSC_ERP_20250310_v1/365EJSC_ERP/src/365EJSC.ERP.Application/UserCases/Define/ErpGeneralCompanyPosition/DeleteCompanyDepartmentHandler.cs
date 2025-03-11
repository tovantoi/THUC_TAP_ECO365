using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Handler for <see cref="DeleteCompanyPositionRequest"/>
    /// </summary>
    public class DeleteCompanyPositionHandler : IRequestHandler<DeleteCompanyPositionRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyPosition"/>>  /// </summary>
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyPositionHandler"/>, inject needed dependency
        /// </summary>
        public DeleteCompanyPositionHandler(ICompanyPositionSqlRepository companyPositionSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.companyPositionSqlRepository = companyPositionSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(DeleteCompanyPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteCompanyPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Entities.ErpGeneralCompanyPosition companyPosition = await companyPositionSqlRepository.FindByIdAsync(request.Id, true, cancellationToken);
            if (companyPosition is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyPosition), MsgCode.ERR_COMPANY_POSITION_ID_NOT_FOUND);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Company as Updated state
                companyPositionSqlRepository.Remove(companyPosition!);

                // Save Company to database
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