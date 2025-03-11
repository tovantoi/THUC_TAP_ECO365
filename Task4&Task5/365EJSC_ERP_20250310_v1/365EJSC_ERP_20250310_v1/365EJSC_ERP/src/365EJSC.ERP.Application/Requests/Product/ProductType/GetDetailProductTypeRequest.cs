using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;

namespace _365EJSC.ERP.Application.Requests.Product.ProductType
{
    public record GetDetailProductTypeRequest : IQuery<PdProductType>
    {
        public int? Id { get; set; }
    }
}
