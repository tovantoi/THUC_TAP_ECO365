using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    public class ErpGeneralPosition : AggregateRoot<int>
    {
        public string? Code { get; set; }
        public string Name { get; set; }
    }
}
