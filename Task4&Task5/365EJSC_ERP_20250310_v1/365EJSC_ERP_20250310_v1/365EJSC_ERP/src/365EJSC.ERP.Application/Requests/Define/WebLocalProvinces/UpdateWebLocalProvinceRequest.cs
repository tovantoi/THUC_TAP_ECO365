using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces
{
    /// <summary>
    /// Request to delete webLocalProvince, contain webLocalProvince id
    /// </summary>
    public record UpdateWebLocalProvinceRequest : ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? KeyLocalization { get; set; }
    }
}
