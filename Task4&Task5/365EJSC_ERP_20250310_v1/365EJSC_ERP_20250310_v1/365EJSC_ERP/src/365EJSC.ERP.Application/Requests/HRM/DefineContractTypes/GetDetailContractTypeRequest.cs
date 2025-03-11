using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes
{
    public record GetDetailContractTypeRequest : IQuery<DefineContractType>
    {
        public int? Id { get; set; }
    }
}
