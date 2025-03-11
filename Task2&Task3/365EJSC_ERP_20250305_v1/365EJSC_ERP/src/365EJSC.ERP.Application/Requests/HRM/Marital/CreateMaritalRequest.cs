using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Marital
{
    public record CreateMaritalRequest : ICommand
    {
        public string? Name { get; set; }
    }
}
