using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IErpGeneralPositionSqlRepository
    /// </summary>
    public class ErpGeneralPositionSqlRepository : GenericSqlRepository<ErpGeneralPosition, int>, IErpGeneralPositionSqlRepository
    {
        /// <summary>
        /// Implementation of IErpGeneralPositionSqlRepository
        /// </summary>
        public ErpGeneralPositionSqlRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
