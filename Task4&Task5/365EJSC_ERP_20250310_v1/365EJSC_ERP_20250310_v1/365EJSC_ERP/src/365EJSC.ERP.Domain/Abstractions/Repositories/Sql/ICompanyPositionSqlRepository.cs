using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql
{
    public interface ICompanyPositionSqlRepository : IGenericSqlRepository<ErpGeneralCompanyPosition, int>
    {
        void AddRange(IEnumerable<ErpGeneralCompanyPosition> entities);
    }
}