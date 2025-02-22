using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IWebLocalWardSqlRepository
    /// </summary>
    public class WebLocalWardSqlRepository : GenericSqlRepository<WebLocalWard, int>, IWebLocalWardSqlRepository
    {
        /// <summary>
        /// Implementation of IWebLocalWardSqlRepository
        /// </summary>
        public WebLocalWardSqlRepository(ApplicationDbContext context) : base(context)
        {
        }
        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<WebLocalWard?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocalWard, object>>[] includeProperties)
        {
            // Call base method
            WebLocalWard? ward = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when ward is null
            if (ward is null)
                CustomException.ThrowNotFoundException(typeof(WebLocalWard), MsgCode.ERR_WARD_ID_NOT_FOUND, WebLocalWardConst.MSG_WARD_ID_NOT_FOUND);

            // Return founded ward
            return ward;
        }

    }
}
