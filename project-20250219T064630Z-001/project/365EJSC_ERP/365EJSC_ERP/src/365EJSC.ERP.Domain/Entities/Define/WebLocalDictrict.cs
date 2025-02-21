using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    public class WebLocalDictrict : AggregateRoot<int>
    {
        public string Name { get; set; }
    }
}
