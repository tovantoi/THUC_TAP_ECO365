using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base
{
    /// <summary>
    /// Provide repository for <see cref="WebLocalDistrict"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IWebLocalDistrictSqlRepository : IGenericSqlRepository<WebLocalDistrict, int>
    {
    }
}
