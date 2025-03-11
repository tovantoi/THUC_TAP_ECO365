using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces
{
    /// <summary>
    /// Request to get existed <see cref="WebLocalProvince"/> by id from database
    /// </summary>
    public record GetDetailWebLocalProvinceRequest : IQuery<WebLocalProvince>
    {
        public int? Id { get; set; }
    }
}
