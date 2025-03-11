using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.HRM.TrainingMajor
{
    /// <summary>
    /// Request to Update a TrainingMajor, contains Id, name
    /// </summary>
    public record UpdateTrainingMajorRequest : ICommand
    {
        /// <summary>
        /// ID of the ward to be updated
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Training Major
        /// </summary>
        public string? TmName { get; set; }

    }
}
