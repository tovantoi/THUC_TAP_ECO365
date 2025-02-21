using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    public class WebLocalWardSqlRepository : GenericSqlRepository<WebLocalWard, int>, IWebLocalWardSqlRepository
    {
        public WebLocalWardSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Add(WebLocalWard entity)
        {
           // entity.CreatedAt = DateTime.UtcNow;
            base.Add(entity);
        }

        public void Update(WebLocalWard entity)
        {
           // entity.UpdatedAt = DateTime.UtcNow;
            base.Update(entity);
        }
        public async Task<WebLocalWard?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocalWard, object>>[] includeProperties)
        {
            // Call base method
            WebLocalWard? ward = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when sample is null
            if (ward is null)
                CustomException.ThrowNotFoundException(typeof(WebLocalWard), MsgCode.ERR_WARD_ID_NOT_FOUND, WebLocalWardConstants.MSG_WARD_ID_NOT_FOUND);

            // Return founded sample
            return ward;
        }
    }
}
