using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants;
using _365EJSC.ERP.Domain.Entities;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{  /// <summary>
   /// Implementation of IWebLocalSqlRepository
   /// </summary>
    public class WeblocalSqlRepository : GenericSqlRepository<WebLocals, string>, IWeblocalSqlRepository
	{
		/// <summary>
		/// Implementation of ISampleRepository
		/// </summary>
		public WeblocalSqlRepository(ApplicationDbContext context) : base(context)
		{
		}

		/// <summary>
		/// Override base method, set CreatedAt to now
		/// </summary>
		/// <param name="entity"></param>
		public void Add(WebLocals entity)
		{
			//entity.CreatedAt = DateTime.UtcNow;
			base.Add(entity);
		}

		/// <summary>
		/// Override base method, set UpdateAt to now
		/// </summary>
		/// <param name="entity"></param>
		public void Update(WebLocals entity)
		{
			//entity.UpdatedAt = DateTime.UtcNow;
			base.Update(entity);
		}

		/// <summary>
		/// Override base method, throw not found exception when entity was not found
		/// </summary>
		/// <param name="id">ID of Domain entity</param>
		/// <param name="cancellationToken"></param>
		/// <param name="includeProperties">Include any relationship if needed</param>
		/// <returns>Domain entity with given id or null if entity with given id not found</returns>
		public async Task<WebLocals?> FindByIdAsync(string id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<WebLocals, object>>[] includeProperties)
		{
			// Call base method
			WebLocals? webLocal = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

			// Throw not found exception when webLocal is null
			if (webLocal is null)
				CustomException.ThrowNotFoundException(typeof(WebLocals), MsgCode.ERR_SAMPLE_ID_NOT_FOUND, WebLocalConst.MSG_KEY_LOCALIZATION_NOT_FOUND);

			// Return founded webLocal
			return webLocal;
		}
	}
}
