using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    public class DefineContractType : AggregateRoot<int>
    {
        public string? Code { get; set; }
        public string Name { get; set; }
    }
}
