using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using _365EJSC.ERP.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ITrainingMajorRepository
    /// </summary>
    public class TrainingMajorSqlRepository : GenericSqlRepository<TrainingMajor, int>, ITrainingMajorSqlRepository
    {
        /// <summary>
        /// Implementation of ITrainingMajorRepository
        /// </summary>
        public TrainingMajorSqlRepository (ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<TrainingMajor?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<TrainingMajor, object>>[] includeProperties)
        {
            // Call base method
            TrainingMajor? TrainingMajor = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when TrainingMajor is null
            if (TrainingMajor is null)
                CustomException.ThrowNotFoundException(typeof(TrainingMajor), MsgCode.ERR_TRAININGMAJOR_ID_NOT_FOUND, TrainingMajorConst.MSG_TRAININGMAJOR_ID_NOT_FOUND);

            // Return found TrainingMajor
            return TrainingMajor;
        }
   
    }
}
