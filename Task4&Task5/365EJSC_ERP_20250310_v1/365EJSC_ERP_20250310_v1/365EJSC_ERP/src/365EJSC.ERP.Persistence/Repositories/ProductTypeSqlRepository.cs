using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IBankSqlRepository
    /// </summary>
    public class ProductTypeSqlRepository : GenericSqlRepository<PdProductType, int>, IProductTypeSqlRepository
    {
        public ProductTypeSqlRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
