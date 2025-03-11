using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces
{
    /// <summary>
    /// Request to create webLocalProvince, contain name, name_en, fullname, fullname_en, latitude, longitude and key_localization
    /// </summary>
    public record CreateWebLocalProvinceRequest : ICommand
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string KeyLocalization { get; set; }
    }
}
