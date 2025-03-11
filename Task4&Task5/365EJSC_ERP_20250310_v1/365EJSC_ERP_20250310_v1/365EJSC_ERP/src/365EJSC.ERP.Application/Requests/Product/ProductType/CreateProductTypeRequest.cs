using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Product.ProductType
{
    public record CreateProductTypeRequest : ICommand
    {
        public string? Name { get; set; }
    }
}
