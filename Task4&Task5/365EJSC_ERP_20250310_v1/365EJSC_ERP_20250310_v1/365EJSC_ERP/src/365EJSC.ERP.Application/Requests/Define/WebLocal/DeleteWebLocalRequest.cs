using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocal
{
    public record DeleteWebLocalRequest : ICommand
    {
        public string? Id { get; set; }
    }
}