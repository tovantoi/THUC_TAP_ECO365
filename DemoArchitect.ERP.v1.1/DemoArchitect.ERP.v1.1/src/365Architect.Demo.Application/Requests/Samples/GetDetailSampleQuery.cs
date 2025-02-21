using _365Architect.Demo.Contract.Abstractions;
using _365Architect.Demo.Domain.Entities;

namespace _365Architect.Demo.Application.Requests.Samples
{
    /// <summary>
    /// Request to get existed <see cref="Sample"/> by id from database
    /// </summary>
    public record GetDetailSampleQuery : IQuery<Sample>
    {
        public int? Id { get; set; }
    }
}