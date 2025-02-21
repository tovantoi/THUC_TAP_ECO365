using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    public record DeleteWebLocalWardRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
