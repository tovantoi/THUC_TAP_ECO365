using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocal
{
    public record UpdateWebLocalRequest : ICommand
    {
        public string? Id { get; set; }
        public string? Localization { get; set; }
        public bool? IsActived { get; set; }
    }
}