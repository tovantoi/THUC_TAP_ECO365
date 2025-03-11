using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql
{   /// <summary>
    /// Provide repository for <see cref="WebLocals"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IWebLocalSqlRepository : IGenericSqlRepository<WebLocals, string>
	{
	}
}