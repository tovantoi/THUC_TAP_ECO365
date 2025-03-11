using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using _365EJSC.ERP.Persistence.Repositories.Base;

namespace _365EJSC.ERP.Persistence.Repositories
{
	public class DegreeSqlRepository(ApplicationDbContext context) : GenericSqlRepository<Degree, int>(context), IDegreeSqlRepository
	{
	}
}
