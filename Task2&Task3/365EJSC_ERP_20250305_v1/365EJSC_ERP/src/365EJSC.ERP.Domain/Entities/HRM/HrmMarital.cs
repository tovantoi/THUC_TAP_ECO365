using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    /// <summary>
    /// Domain entity for Marital with int key type
    /// </summary>
    public class HrmMarital : AggregateRoot<int>
    {
        /// <summary>
        /// Name of the marital
        /// </summary>
        public string Name { get; set; }
    }
}
