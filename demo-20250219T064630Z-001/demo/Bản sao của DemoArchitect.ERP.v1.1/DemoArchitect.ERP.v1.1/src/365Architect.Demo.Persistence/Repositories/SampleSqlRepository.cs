using System.Linq.Expressions;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Constants;
using _365Architect.Demo.Domain.Entities;
using _365Architect.Demo.Persistence.Repositories.Base;

namespace _365Architect.Demo.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class SampleSqlRepository : GenericSqlRepository<Sample, int>, ISampleSqlRepository
    {
        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public SampleSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override base method, set CreatedAt to now
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Sample entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            base.Add(entity);
        }

        /// <summary>
        /// Override base method, set UpdateAt to now
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Sample entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            base.Update(entity);
        }

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<Sample?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<Sample, object>>[] includeProperties)
        {
            // Call base method
            Sample? sample = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when sample is null
            if (sample is null)
                CustomException.ThrowNotFoundException(typeof(Sample), MsgCode.ERR_SAMPLE_ID_NOT_FOUND, SampleConst.MSG_SAMPLE_ID_NOT_FOUND);

            // Return founded sample
            return sample;
        }
    }
}