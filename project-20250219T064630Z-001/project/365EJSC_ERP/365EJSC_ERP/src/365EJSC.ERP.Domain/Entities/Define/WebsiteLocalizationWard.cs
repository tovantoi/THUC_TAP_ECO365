using _365EJSC.ERP.Domain.Abstractions.Aggregates;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    public class WebsiteLocalizationWard : AggregateRoot<int>
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int DistrictId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WebsiteLocalizationDictrict? WebsiteLocalizationDictrict { get; set; }

    }
}
