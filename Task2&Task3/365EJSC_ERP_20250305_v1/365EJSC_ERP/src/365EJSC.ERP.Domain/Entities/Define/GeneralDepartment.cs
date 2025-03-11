using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    public class GeneralDepartment : AggregateRoot<int>
    {
        /// <summary>
        /// Department code
        /// </summary>
        public string DeCode { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string DeName { get; set; }

        /// <summary>
        /// Indicates whether the department is active
        /// </summary>
        public bool IsActived { get; set; }
    }
}
