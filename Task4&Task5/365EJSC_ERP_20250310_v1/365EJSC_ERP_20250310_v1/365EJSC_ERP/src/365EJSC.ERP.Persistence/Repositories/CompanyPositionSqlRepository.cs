using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
    public class CompanyPositionSqlRepository(ApplicationDbContext context) : GenericSqlRepository<ErpGeneralCompanyPosition, int>(context), ICompanyPositionSqlRepository
    {
        public void AddRange(IEnumerable<ErpGeneralCompanyPosition> entities)
        {
            Entities.AddRange(entities);
        }
    }
}