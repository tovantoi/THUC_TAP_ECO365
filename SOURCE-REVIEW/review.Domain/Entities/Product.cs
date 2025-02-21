using review.Domain.Abstractions.Aggregates;

namespace review.Domain.Entities
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Product : AggregateRoot<int>
    {
        /// <summary>
        /// Title of sample
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of sample
        /// </summary>
        public string? Description { get; set; }

    }
}