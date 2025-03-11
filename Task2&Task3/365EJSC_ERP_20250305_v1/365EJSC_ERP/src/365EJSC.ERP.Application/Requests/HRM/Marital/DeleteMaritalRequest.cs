using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Marital
{
    public record DeleteMaritalRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
