using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.Marital
{
    public record GetDetailMaritalRequest : IQuery<HrmMarital>
    {
        public int? Id { get; set; }
    }
}
