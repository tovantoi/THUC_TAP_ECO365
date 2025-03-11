using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces
{
    /// <summary>
    /// Request to get all existed <see cref="WebLocalProvince"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllWebLocalProvinceRequest : IQuery<List<WebLocalProvince>>
    {
        public string? KeyLocalization {  get; set; }
    }
}
