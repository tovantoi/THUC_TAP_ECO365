using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Bank
{
    public record DeleteBankRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
