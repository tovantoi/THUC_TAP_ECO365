using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    /// <summary>
    /// Domain entity for WebLocalWard with int key type
    /// </summary>
    public class WebLocalWard : AggregateRoot<int>
    {
        /// <summary>
        /// Name of WebLocalWard
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NameEn of WebLocalWard
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// FullName of WebLocalWard
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// FullNameEn of WebLocalWard
        /// </summary>
        public string FullNameEn { get; set; }
        /// <summary>
        /// Latitude of WebLocalWard
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of WebLocalWard
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// DistrictId of WebLocalWard
        /// </summary>
        public int DistrictId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WebLocalDistrict? WebLocalDistrict { get; set; }
    }
}
