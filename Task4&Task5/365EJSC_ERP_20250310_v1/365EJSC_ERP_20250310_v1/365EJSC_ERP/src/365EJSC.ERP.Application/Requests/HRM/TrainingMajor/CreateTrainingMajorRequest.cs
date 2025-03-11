using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.TrainingMajor
{
    /// <summary>
    /// Request to create a TrainingMajor, contains name
    /// </summary>
    public record CreateTrainingMajorRequest : ICommand
    {
        /// <summary>
        /// Name of the Training Major
        /// </summary>
        public string? TmName { get; set; }

    }
}
