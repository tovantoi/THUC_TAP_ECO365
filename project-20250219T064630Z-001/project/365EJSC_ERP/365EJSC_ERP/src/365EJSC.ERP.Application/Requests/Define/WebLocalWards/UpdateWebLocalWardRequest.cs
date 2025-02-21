using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    public record UpdateWebLocalWardRequest : ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? DistrictId { get; set; }
    }
}
