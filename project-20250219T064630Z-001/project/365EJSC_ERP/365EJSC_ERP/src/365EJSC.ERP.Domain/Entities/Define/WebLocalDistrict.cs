using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    /// <summary>
    /// Domain entity for District with int key type
    /// </summary>
    public class WebLocalDistrict : AggregateRoot<int>
    {
        /// <summary>
        /// Name of the district
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the district in English
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Full name of the district
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Full name of the district in English
        /// </summary>
        public string FullNameEn { get; set; }

        /// <summary>
        /// Latitude of the district
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude of the district
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Province ID the district belongs to
        /// </summary>
        public int ProvinceId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WebLocalProvince? WebLocalProvince { get; set; }

    }
}
