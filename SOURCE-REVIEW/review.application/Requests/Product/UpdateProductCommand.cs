using review.Contract.Abstractions;

namespace review.application.Requests.Product
{
    /// <summary>
    /// Request to delete sample, contain sample id
    /// </summary>
    public record UpdateProductCommand : ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}