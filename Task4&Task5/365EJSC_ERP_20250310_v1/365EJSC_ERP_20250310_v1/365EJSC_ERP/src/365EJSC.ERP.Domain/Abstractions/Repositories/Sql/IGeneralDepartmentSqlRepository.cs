using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql
{
    public interface IGeneralDepartmentSqlRepository : IGenericSqlRepository<GeneralDepartment, int>
    {
        Task<List<GeneralDepartment>> FindByIds(IEnumerable<int> ids, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<GeneralDepartment, object>>[] includeProperties);
    }
}
