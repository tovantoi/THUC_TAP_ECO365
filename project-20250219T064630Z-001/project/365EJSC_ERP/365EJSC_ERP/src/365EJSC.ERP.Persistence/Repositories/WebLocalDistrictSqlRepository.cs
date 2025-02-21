using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants.Define.WebLocalDistricts;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IDistrictRepository
    /// </summary>
    public class WebLocalDistrictSqlRepository : GenericSqlRepository<WebLocalDistrict, int>, IWebLocalDistrictSqlRepository
    {
        /// <summary>
        /// Implementation of IDistrictRepository
        /// </summary>
        public WebLocalDistrictSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<WebLocalDistrict?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocalDistrict, object>>[] includeProperties)
        {
            // Call base method
            WebLocalDistrict? district = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when district is null
            if (district is null)
                CustomException.ThrowNotFoundException(typeof(WebLocalDistrict), MsgCode.ERR_DISTRICT_ID_NOT_FOUND, WebLocalDistrictConst.MSG_DISTRICT_ID_NOT_FOUND);

            // Return found district
            return district;
        }
    }
}
