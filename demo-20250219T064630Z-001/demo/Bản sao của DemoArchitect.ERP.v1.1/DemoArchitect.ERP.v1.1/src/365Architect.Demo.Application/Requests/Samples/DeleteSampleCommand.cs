using _365Architect.Demo.Contract.Abstractions;

namespace _365Architect.Demo.Application.Requests.Samples
{
    /// <summary>
    /// Request to delete sample, contain sample id
    /// </summary>
    public record DeleteSampleCommand : ICommand
    {
        public int? Id { get; set; }
    }
}