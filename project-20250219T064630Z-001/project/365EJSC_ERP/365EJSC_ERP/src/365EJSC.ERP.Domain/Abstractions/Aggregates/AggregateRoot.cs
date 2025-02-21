using _365EJSC.ERP.Domain.Abstractions.Entities;

namespace _365EJSC.ERP.Domain.Abstractions.Aggregates
{
    /// <summary>
    /// Aggregate root
    /// </summary>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
    }
}