using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Constants.Define;
using Microsoft.EntityFrameworkCore;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IGeneralDepartmentRepository
    /// </summary>
    public class GeneralDepartmentSqlRepository : GenericSqlRepository<GeneralDepartment, int>, IGeneralDepartmentSqlRepository
    {
        /// <summary>
        /// Implementation of IGeneralDepartmentRepository
        /// </summary>
        public GeneralDepartmentSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<GeneralDepartment?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<GeneralDepartment, object>>[] includeProperties)
        {
            // Call base method
            GeneralDepartment? GeneralDepartment = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when GeneralDepartment is null
            if (GeneralDepartment is null)
                CustomException.ThrowNotFoundException(typeof(GeneralDepartment), MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND, GeneralDepartmentConst.MSG_DEPARTMENT_ID_NOT_FOUND);

            // Return found GeneralDepartment
            return GeneralDepartment;
        }

        public async Task<List<GeneralDepartment>> FindByIds(IEnumerable<int> ids, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<GeneralDepartment, object>>[] includeProperties)
        {
            var query = Entities.AsQueryable();

            if (includeProperties.Any())
                query = IncludeMultiple(query, includeProperties);

            query = isTracking ? query : query.AsNoTracking();

            var result = await query.Where(x => ids.Contains(x.Id!)).ToListAsync(cancellationToken);
            return result;
        }

        private IQueryable<GeneralDepartment> IncludeMultiple(IQueryable<GeneralDepartment> source, params Expression<Func<GeneralDepartment, object>>[] includeProperties)
        {
            if (includeProperties.Any())
                // Each property will be included into source
                source = includeProperties.Aggregate(source, (current, include) => current.Include(include));
            return source;
        }
    }
}
