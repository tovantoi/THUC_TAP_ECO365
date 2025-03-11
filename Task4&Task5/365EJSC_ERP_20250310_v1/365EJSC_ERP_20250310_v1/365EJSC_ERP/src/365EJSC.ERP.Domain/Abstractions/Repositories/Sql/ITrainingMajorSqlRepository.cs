using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Abstractions.Repositories.Sql
{
    /// <summary>
    /// Provide repository for <see cref="TrainingMajor"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface ITrainingMajorSqlRepository : IGenericSqlRepository<TrainingMajor, int>
    {
    }
}
