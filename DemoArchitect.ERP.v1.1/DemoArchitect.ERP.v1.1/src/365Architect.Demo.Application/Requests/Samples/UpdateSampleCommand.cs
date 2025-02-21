using _365Architect.Demo.Contract.Abstractions;

namespace _365Architect.Demo.Application.Requests.Samples
{
    /// <summary>
    /// Request to delete sample, contain sample id
    /// </summary>
    public record UpdateSampleCommand : ICommand
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}