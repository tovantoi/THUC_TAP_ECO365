using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.HRM.TrainingMajor
{
    /// <summary>
    /// Request to delete a WebLocalWard by its ID
    /// </summary>
    public record DeleteTrainingMajorRequest : ICommand
    {
        /// <summary>
        /// ID of the ward to be deleted
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }
    }
}
