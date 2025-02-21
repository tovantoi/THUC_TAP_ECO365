using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class WebLocalProvince : AggregateRoot<int>
    {
        /// <summary>
        /// Name of webLocalProvince
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NameEn of webLocalProvince
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// FullName of webLocalProvince
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// FullNameEn of webLocalProvince
        /// </summary>
        public string FullNameEn { get; set; }
        /// <summary>
        /// Latitude of webLocalProvince
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of webLocalProvince
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// KeyLocalization of webLocalProvince
        /// </summary>
        public string KeyLocalization { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WebLocals? WebLocals { get; set; }

    }
}
