using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole
{
    public class GetDetailEmployeeRoleHandler : IRequestHandler<GetDetailEmployeeRoleRequest, Result<HrmEmployeeRole>>
    {
        private readonly IEmployeeRoleSqlRepository employeeRoleSqlRepository;

        public GetDetailEmployeeRoleHandler(IEmployeeRoleSqlRepository employeeRoleSqlRepository)
        {
            this.employeeRoleSqlRepository = employeeRoleSqlRepository;
        }
        public async Task<Result<HrmEmployeeRole>> Handle(GetDetailEmployeeRoleRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailEmployeeRoleValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find employee by id provided. If employee not found will throw NotFoundException
            HrmEmployeeRole? employee = await employeeRoleSqlRepository.FindByIdAsync(request.Id.Value, false, cancellationToken);
            if (employee is null) CustomException.ThrowNotFoundException(typeof(HrmEmployeeRole), MsgCode.ERR_EMPLOYEE_ID_NOT_FOUND);

            return employee;
        }
    }
}
