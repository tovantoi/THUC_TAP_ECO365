using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define
{   /// <summary>
    /// Provide repository for <see cref="WebLocals"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IWeblocalSqlRepository : IGenericSqlRepository<WebLocals, string>
    {
    }
}
