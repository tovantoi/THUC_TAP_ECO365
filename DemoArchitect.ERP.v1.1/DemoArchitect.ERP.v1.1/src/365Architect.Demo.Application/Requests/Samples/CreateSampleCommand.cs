using _365Architect.Demo.Contract.Abstractions;

namespace _365Architect.Demo.Application.Requests.Samples
{
    /// <summary>
    /// Request to create sample, contain title, description and due date
    /// </summary>
    public record CreateSampleCommand : ICommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        public DateTime? DueDate { get; set; }
    }
}