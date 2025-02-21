using review.Contract.Abstractions;

namespace review.application.Requests.Product
{
    /// <summary>
    /// Request to delete sample, contain sample id
    /// </summary>
    public record DeleteProductCommand : ICommand
    {
        public int? Id { get; set; }
    }
}