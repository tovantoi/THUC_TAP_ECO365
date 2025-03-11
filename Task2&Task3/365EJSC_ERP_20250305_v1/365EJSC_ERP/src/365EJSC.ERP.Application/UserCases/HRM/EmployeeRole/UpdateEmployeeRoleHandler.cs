using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole
{
    public class UpdateEmployeeRoleHandler : IRequestHandler<UpdateEmployeeRoleRequest, Result<object>>
    {
        private readonly IEmployeeRoleSqlRepository employeeRoleSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public UpdateEmployeeRoleHandler(ISqlUnitOfWork sqlUnitOfWork, IEmployeeRoleSqlRepository employeeRoleSqlRepository)
        {
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.employeeRoleSqlRepository = employeeRoleSqlRepository;
        }
        /// <summary>
        /// Handle <see cref="UpdateEmployeeRoleRequest"/>, find existing <see cref="HrmMaritals"/> base on id provided in <see cref="UpdateEmployeeRoleRequest"/>,
        /// update founded <see cref="HrmMaritals"/> base on data provided in <see cref="UpdateEmployeeRoleRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateEmployeeRoleRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateEmployeeRoleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find marital base on id provided from database, if marital was not found, throw not found exception.
            // Need tracking to update Local.
            HrmEmployeeRole? employee = await employeeRoleSqlRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
            if (employee is null) CustomException.ThrowNotFoundException(typeof(HrmEmployeeRole), MsgCode.ERR_EMPLOYEE_ID_NOT_FOUND);

            // Update marital base on data provided in UpdateHrmMaritalRequest.
            // Keep marital original data if request fields is null
            request.MapTo(employee, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark marital as Updated state
                employeeRoleSqlRepository.Update(employee!);

                // Save marital to database
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
