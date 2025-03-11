using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.Product.ProductType
{
    public class PdProductType : AggregateRoot<int>
    {
        public string Name { get; set; }
    }
}
