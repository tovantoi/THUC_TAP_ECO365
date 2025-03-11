using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.Bank
{
    public record GetDetailBankRequest : IQuery<HrmBank>
    {
        public int? Id { get; set; }
    }
}
