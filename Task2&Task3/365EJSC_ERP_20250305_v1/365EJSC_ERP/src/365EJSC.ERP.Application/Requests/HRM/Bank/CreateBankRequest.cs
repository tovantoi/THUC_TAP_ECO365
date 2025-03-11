using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Bank
{
    public record CreateBankRequest : ICommand
    {
        public string? Name { get; set; }
    }
}
