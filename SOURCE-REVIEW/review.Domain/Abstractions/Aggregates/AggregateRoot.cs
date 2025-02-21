using review.Domain.Abstractions.Entities;

namespace review.Domain.Abstractions.Aggregates
{
    /// <summary>
    /// Aggregate root
    /// </summary>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
    }
}