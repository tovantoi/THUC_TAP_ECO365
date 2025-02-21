using _365Architect.Demo.Domain.Abstractions.Aggregates;

namespace _365Architect.Demo.Domain.Entities
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Sample : AggregateRoot<int>
    {
        /// <summary>
        /// Title of sample
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of sample
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Due date of sample
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Created time of Sample
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last nearest updated time of sample
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}