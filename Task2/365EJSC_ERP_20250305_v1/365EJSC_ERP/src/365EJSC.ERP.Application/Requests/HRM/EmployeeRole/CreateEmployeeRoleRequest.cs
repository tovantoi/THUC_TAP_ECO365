using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.EmployeeRole
{
    public record CreateEmployeeRoleRequest : ICommand
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
