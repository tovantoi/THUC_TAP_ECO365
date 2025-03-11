using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class DefineSalaryStructure : AggregateRoot<int>
    {
        public string? Code { get; set; }
        public string Name { get; set; }
    }
}