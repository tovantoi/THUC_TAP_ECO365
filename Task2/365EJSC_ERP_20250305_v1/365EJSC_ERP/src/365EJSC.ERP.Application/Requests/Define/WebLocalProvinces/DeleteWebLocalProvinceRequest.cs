using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces
{
    /// <summary>
    /// Request to delete webLocalProvince, contain webLocalProvince id
    /// </summary>
    public record DeleteWebLocalProvinceRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
