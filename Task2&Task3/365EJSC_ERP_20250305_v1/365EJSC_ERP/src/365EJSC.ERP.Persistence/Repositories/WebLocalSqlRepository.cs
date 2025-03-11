using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{  /// <summary>
   /// Implementation of IWebLocalSqlRepository
   /// </summary>
    public class WebLocalSqlRepository : GenericSqlRepository<WebLocals, string>, IWebLocalSqlRepository
	{
		/// <summary>
		/// Implementation of ISampleRepository
		/// </summary>
		public WebLocalSqlRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}