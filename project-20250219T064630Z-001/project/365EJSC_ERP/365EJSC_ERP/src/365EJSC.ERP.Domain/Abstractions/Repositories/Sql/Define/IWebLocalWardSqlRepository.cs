using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define
{
    /// <summary>
    /// Provide repository for <see cref="WebLocalWard"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IWebLocalWardSqlRepository : IGenericSqlRepository<WebLocalWard, int>
    {
    }
}
