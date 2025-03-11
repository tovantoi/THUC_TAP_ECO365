using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class ErpGeneralCompanyDepartment : AggregateRoot<int>
    {
        /// <summary>
        /// Company ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Department ID
        /// </summary>
        public int DepartmentId { get; set; }
    }
}