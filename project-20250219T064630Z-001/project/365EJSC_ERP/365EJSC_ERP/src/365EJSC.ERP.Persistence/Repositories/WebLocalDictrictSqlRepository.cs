using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    public class WebLocalDictrictSqlRepository : GenericSqlRepository<WebLocalDictrict, int>, IWebLocalDictrictSqlRepository
    {
        public WebLocalDictrictSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Add(WebLocalDictrict entity)
        {
            // entity.CreatedAt = DateTime.UtcNow;
            base.Add(entity);
        }

        public void Update(WebLocalDictrict entity)
        {
            // entity.UpdatedAt = DateTime.UtcNow;
            base.Update(entity);
        }
        public async Task<WebLocalDictrict?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocalDictrict, object>>[] includeProperties)
        {
            // Call base method
            WebLocalDictrict? ward = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when sample is null
            if (ward is null)
                CustomException.ThrowNotFoundException(typeof(WebLocalDictrict), MsgCode.ERR_DICTRICT_ID_NOT_FOUND, WebLocalDictrictConstants.MSG_DICTRICT_ID_NOT_FOUND);

            // Return founded sample
            return ward;
        }
    }
}
