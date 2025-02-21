using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWard
{
    public record DeleteWardCommand : ICommand
    {
        public int? Id { get; set; }
    }
}
