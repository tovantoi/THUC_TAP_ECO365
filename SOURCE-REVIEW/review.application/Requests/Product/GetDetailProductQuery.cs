using review.Contract.Abstractions;
using review.Domain.Entities;

namespace review.application.Requests.Product
{
    /// <summary>
    /// Request to get existed <see cref="Sample"/> by id from database
    /// </summary>
    public record GetDetailProductQuery : IQuery<Domain.Entities.Product>
    {
        public int? Id { get; set; }
    }
}