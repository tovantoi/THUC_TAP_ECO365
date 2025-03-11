using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.HRM
{
	public class Degree : AggregateRoot<int>
	{ 
        /// <summary>
        /// Description of the location
        /// </summary>
        public string? Name { get; set; }
}
}
