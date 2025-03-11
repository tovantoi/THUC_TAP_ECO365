using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    public class WebLocals : AggregateRoot<string>
    {
        /// <summary>
        /// Description of the location
        /// </summary>
        public string? Localization { get; set; }

        /// <summary>
        /// Status of the localization (True: Active, False: Inactive)
        /// </summary>
        public bool IsActived { get; set; }
    }
}