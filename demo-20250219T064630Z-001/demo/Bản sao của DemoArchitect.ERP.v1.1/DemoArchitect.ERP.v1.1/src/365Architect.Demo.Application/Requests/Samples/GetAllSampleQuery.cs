using _365Architect.Demo.Contract.Abstractions;
using _365Architect.Demo.Domain.Entities;

namespace _365Architect.Demo.Application.Requests.Samples
{
    /// <summary>
    /// Request to get all existed <see cref="Sample"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllSampleQuery : IQuery<List<Sample>>
    {
    }
}