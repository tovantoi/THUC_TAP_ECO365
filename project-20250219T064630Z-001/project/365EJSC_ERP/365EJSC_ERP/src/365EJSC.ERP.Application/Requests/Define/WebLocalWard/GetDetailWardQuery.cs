using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWard
{
    public record GetDetailWardQuery : IQuery<WebsiteLocalizationWard>
    {
        public int? Id { get; set; }
    }
}
