using review.Contract.Abstractions;

namespace review.application.Requests.Product
{
    /// <summary>
    /// Request to create sample, contain title, description and due date
    /// </summary>
    public record CreateProductCommand : ICommand
    {
        public string? Name { get; set; }
        public string? Description { get; set; } = null;
    }
}