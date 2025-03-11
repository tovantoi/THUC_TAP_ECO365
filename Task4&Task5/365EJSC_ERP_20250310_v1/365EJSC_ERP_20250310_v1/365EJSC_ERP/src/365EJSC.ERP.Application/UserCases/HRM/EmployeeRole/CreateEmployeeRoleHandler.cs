using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole
{
    public class CreateEmployeeRoleHandler : IRequestHandler<CreateEmployeeRoleRequest, Result<object>>
    {
        private readonly IEmployeeRoleSqlRepository employeeRoleSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateEmployeeRoleHandler(IEmployeeRoleSqlRepository employeeRoleSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.employeeRoleSqlRepository = employeeRoleSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }
        public async Task<Result<object>> Handle(CreateEmployeeRoleRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateEmployeeRoleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new employee from request
            HrmEmployeeRole? employee = request.MapTo<HrmEmployeeRole>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked employee as Created state
                employeeRoleSqlRepository.Add(employee);

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
