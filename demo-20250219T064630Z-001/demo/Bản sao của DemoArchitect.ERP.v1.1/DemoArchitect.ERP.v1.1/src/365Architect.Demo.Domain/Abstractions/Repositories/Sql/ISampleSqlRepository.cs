using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;

namespace _365Architect.Demo.Domain.Abstractions.Repositories.Sql
{
    /// <summary>
    /// Provide repository for <see cref="Sample"/>, inherit from <see cref="IGenericSqlRepository{TEntity,TKey}"/>
    /// </summary>
    public interface ISampleSqlRepository : IGenericSqlRepository<Sample, int>
    {
    }
}