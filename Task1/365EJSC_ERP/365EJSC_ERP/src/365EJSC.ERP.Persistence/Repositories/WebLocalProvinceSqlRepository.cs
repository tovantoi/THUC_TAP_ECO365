using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

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

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<WebLocalProvince?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocalProvince, object>>[] includeProperties)
        {
            // Call base method
            WebLocalProvince? webLocalProvince = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when webLocalProvince is null
            if (webLocalProvince is null)
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_SAMPLE_ID_NOT_FOUND, WebLocalProvinceConst.MSG_LOCALPROVINCE_ID_NOT_FOUND);

            // Return founded webLocalProvince
            return webLocalProvince;
        }
    }
}
