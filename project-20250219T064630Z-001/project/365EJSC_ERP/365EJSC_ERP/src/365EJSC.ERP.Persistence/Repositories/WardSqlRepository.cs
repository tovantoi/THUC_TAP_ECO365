using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    public class WardSqlRepository : GenericSqlRepository<WebsiteLocalizationWard, int>, IWardSqlRepository
    {
        public WardSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Add(WebsiteLocalizationWard entity)
        {
           // entity.CreatedAt = DateTime.UtcNow;
            base.Add(entity);
        }

        public void Update(WebsiteLocalizationWard entity)
        {
           // entity.UpdatedAt = DateTime.UtcNow;
            base.Update(entity);
        }
        public async Task<WebsiteLocalizationWard?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebsiteLocalizationWard, object>>[] includeProperties)
        {
            // Call base method
            WebsiteLocalizationWard? ward = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when sample is null
            if (ward is null)
                CustomException.ThrowNotFoundException(typeof(WebsiteLocalizationWard), MsgCode.ERR_WARD_ID_NOT_FOUND, WebsiteLocalizationWardConstants.MSG_WARD_ID_NOT_FOUND);

            // Return founded sample
            return ward;
        }
    }
}
