using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Handler for <see cref="DeleteCompanyDepartmentRequest"/>
    /// </summary>
    public class DeleteCompanyDepartmentHandler : IRequestHandler<DeleteCompanyDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyDepartment"/>>  /// </summary>
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public DeleteCompanyDepartmentHandler(ICompanyDepartmentSqlRepository companyDepartmentSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(DeleteCompanyDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteCompanyDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Entities.ErpGeneralCompanyDepartment companyDepartment = await companyDepartmentSqlRepository.FindByIdAsync(request.Id, true, cancellationToken);
            if (companyDepartment is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyDepartment), MsgCode.ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Company as Updated state
                companyDepartmentSqlRepository.Remove(companyDepartment!);

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