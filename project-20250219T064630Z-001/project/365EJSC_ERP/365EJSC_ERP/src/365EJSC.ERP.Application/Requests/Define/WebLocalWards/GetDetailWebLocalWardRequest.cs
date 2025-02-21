using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    public record GetDetailWebLocalWardRequest : IQuery<Domain.Entities.Define.WebLocalWard>
    {
        public int? Id { get; set; }
    }
}
