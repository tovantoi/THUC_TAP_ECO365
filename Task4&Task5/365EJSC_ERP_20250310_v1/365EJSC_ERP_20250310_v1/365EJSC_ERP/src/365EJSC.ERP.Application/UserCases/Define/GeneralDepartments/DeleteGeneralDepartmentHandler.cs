using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.Validators.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments
{
    /// <summary>
    /// Handler for <see cref="DeleteGeneralDepartmentRequest"/>
    /// </summary>
    public class DeleteGeneralDepartmentHandler : IRequestHandler<DeleteGeneralDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="GeneralDepartment"/>
        /// </summary>
        private readonly IGeneralDepartmentSqlRepository generalDepartmentSqlRepository;
       
        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteGeneralDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public DeleteGeneralDepartmentHandler(IGeneralDepartmentSqlRepository generalDepartmentSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.generalDepartmentSqlRepository = generalDepartmentSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteGeneralDepartmentRequest"/>, find existing <see cref="GeneralDepartment"/> based on id provided in <see cref="DeleteGeneralDepartmentRequest"/>,
        /// delete the found <see cref="GeneralDepartment"/> and save to the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteGeneralDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteGeneralDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find general department based on id provided from database, if general department was not found, throw not found exception.
            // Need tracking to delete general department.
            GeneralDepartment department = await generalDepartmentSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Check if IsActived is 0 before deletion
            if (department.IsActived != true)
            {
                CustomException.ThrowValidationException(
                    MsgCode.INF_DELETED,
                    GeneralDepartmentConst.INACTIVE_DEPARTMENT_REQUIRED_MSG
                );
            }

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked general department as Deleted state
                generalDepartmentSqlRepository.Remove(department);

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
