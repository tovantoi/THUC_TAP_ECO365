using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ICompanyDepartmentRepository
    /// </summary>
    public class CompanyDepartmentSqlRepository(ApplicationDbContext context) : GenericSqlRepository<ErpGeneralCompanyDepartment, int>(context), ICompanyDepartmentSqlRepository
    {
        public void AddRange(IEnumerable<ErpGeneralCompanyDepartment> entities)
        {
            Entities.AddRange(entities);
        }
    }
}