namespace _365EJSC.ERP.Domain.Abstractions.Entities
{
    /// <summary>
    /// Domain entity
    /// </summary>
    public abstract class Entity<TKey> : IEntity
    {
        /// <summary>
        /// Primary key of entity
        /// </summary>
        public TKey Id { get; set; }
    }
}