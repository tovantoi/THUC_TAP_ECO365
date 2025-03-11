using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole
{
    public class GetAllEmployeeRoleHandler : IRequestHandler<GetAllEmployeeRoleRequest, Result<List<HrmEmployeeRole>>>
    {
        private readonly IEmployeeRoleSqlRepository employeeRoleSqlRepository;

        public GetAllEmployeeRoleHandler(IEmployeeRoleSqlRepository employeeRoleSqlRepository)
        {
            this.employeeRoleSqlRepository = employeeRoleSqlRepository;
        }

        public Task<Result<List<HrmEmployeeRole>>> Handle(GetAllEmployeeRoleRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<HrmEmployeeRole>>>(employeeRoleSqlRepository.FindAll().ToList());
        }
    }
}
