using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    /// <summary>
    /// Domain entity for Marital with int key type
    /// </summary>
    public class HrmEmployeeRole : AggregateRoot<int>
    {
        /// <summary>
        /// Code of the marital
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the marital
        /// </summary>
        public string Name { get; set; }
    }
}
