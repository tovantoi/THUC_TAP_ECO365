using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.EmployeeRole
{
    public record DeleteEmployeeRoleRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
