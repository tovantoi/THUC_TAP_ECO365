using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    public class HrmBank : AggregateRoot<int>
    {
        public string Name { get; set; }
    }
}
