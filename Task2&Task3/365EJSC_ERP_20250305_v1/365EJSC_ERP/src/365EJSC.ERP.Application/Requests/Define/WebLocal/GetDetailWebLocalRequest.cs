using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocal
{
    public record GetDetailWebLocalRequest : IQuery<WebLocals>
    {
        public string? Id { get; set; }
    }
}