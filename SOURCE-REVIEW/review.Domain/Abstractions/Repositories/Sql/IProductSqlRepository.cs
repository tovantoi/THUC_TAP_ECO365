using review.Domain.Abstractions.Repositories.Sql.Base;
using review.Domain.Entities;

namespace review.Domain.Abstractions.Repositories.Sql
{
    /// <summary>
    /// Provide repository for <see cref="Product"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IProductSqlRepository : IGenericSqlRepository<Product, int>
    {
    }
}