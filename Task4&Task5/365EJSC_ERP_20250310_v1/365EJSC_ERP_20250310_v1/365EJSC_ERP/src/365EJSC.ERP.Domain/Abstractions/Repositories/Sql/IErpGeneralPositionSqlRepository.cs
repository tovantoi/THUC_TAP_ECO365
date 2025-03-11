using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql
{
    /// <summary>
    /// Provide repository for <see cref="ErpGeneralPosition"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface IErpGeneralPositionSqlRepository : IGenericSqlRepository<ErpGeneralPosition, int>
    {
        Task<List<ErpGeneralPosition>> FindByIds(IEnumerable<int> ids, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<ErpGeneralPosition, object>>[] includeProperties);
    }
}
