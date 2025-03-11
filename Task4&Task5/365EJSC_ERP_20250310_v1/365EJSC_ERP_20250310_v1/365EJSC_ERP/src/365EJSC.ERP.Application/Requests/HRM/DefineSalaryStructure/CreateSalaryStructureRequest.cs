using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Request to create SalaryStructure, contain code and name
    /// </summary>
    public record CreateSalaryStructureRequest : ICommand
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}