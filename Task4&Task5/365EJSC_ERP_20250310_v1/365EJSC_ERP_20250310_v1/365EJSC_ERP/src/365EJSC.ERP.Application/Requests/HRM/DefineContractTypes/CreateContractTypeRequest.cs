using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes
{
    public record CreateContractTypeRequest : ICommand
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
