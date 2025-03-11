using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<List<ErpGeneralPosition>> FindByIds(IEnumerable<int> ids, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<ErpGeneralPosition, object>>[] includeProperties)
        {
            var query = Entities.AsQueryable();

            if (includeProperties.Any())
                query = IncludeMultiple(query, includeProperties);

            query = isTracking ? query : query.AsNoTracking();

            var result = await query.Where(x => ids.Contains(x.Id!)).ToListAsync(cancellationToken);
            return result;
        }

        private IQueryable<ErpGeneralPosition> IncludeMultiple(IQueryable<ErpGeneralPosition> source, params Expression<Func<ErpGeneralPosition, object>>[] includeProperties)
        {
            if (includeProperties.Any())
                // Each property will be included into source
                source = includeProperties.Aggregate(source, (current, include) => current.Include(include));
            return source;
        }
    }
}
