using review.Contract.Abstractions;
using review.Domain.Entities;

namespace review.application.Requests.Product
{
    /// <summary>
    /// Request to get all existed <see cref="Sample"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllProductsQuery : IQuery<List<Domain.Entities.Product>>
    {
    }
}