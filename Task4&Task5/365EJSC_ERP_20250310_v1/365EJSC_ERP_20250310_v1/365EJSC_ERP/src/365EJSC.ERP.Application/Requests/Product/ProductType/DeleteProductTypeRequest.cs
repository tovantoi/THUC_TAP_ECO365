using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Product.ProductType
{
    public record DeleteProductTypeRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
