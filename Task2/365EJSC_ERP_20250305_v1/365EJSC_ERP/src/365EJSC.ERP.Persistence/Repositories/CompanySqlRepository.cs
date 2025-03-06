using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ICompanyRepository
    /// </summary>
    public class CompanySqlRepository(ApplicationDbContext context) : GenericSqlRepository<ErpGeneralCompany, int>(context), ICompanySqlRepository
    {
    }
}