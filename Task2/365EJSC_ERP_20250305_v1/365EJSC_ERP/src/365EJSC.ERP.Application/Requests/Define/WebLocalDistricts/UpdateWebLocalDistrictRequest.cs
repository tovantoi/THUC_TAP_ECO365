using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts
{
    /// <summary>
    /// Request to update district, containing district id and other fields
    /// </summary>
    public record UpdateWebLocalDistrictRequest : ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? ProvinceId { get; set; }
    }

}
