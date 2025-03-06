using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IWebLocalProvinceSqlRepository
    /// </summary>
    public class WebLocalProvinceSqlRepository : GenericSqlRepository<WebLocalProvince, int>, IWebLocalProvinceSqlRepository
    {
        /// <summary>
        /// Implementation of IWebLocalProvinceSqlRepository
        /// </summary>
        public WebLocalProvinceSqlRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
