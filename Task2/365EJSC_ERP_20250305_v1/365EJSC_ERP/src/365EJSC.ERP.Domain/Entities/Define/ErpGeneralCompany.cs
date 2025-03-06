using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class ErpGeneralCompany : AggregateRoot<int>
    {
        /// <summary>
        /// Company PID
        /// </summary>
        public int CompanyPid { get; set; }

        /// <summary>
        /// Tax code of the company
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image URL of the company
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Telephone number of the company
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Email address of the company
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Website URL of the company
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Name of the company's founder
        /// </summary>
        public string? Founder { get; set; }

        /// <summary>
        /// Name of the company's CEO
        /// </summary>
        public string Ceo { get; set; }

        /// <summary>
        /// Image URL of the company's CEO
        /// </summary>
        public string? CeoImage { get; set; }

        /// <summary>
        /// Email address of the company's CEO
        /// </summary>
        public string? CeoEmail { get; set; }

        /// <summary>
        /// Telephone number of the company's CEO
        /// </summary>
        public string? CeoTel { get; set; }

        /// <summary>
        /// Company license number
        /// </summary>
        public string? License { get; set; }

        /// <summary>
        /// Country ID where the company is located
        /// </summary>
        public string? CountryId { get; set; }

        /// <summary>
        /// Ward ID where the company is located
        /// </summary>
        public int? WardId { get; set; }

        /// <summary>
        /// Indicates whether the company is active (1 = active, 0 = inactive)
        /// </summary>
        public bool IsActived { get; set; }
    }
}