using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    public class TrainingMajor : AggregateRoot<int>
    {

        /// Name of Training Major
        /// </summary>
        public string? TmName { get; set; }
    }
}
