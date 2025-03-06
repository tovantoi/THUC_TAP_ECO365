using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts
{
    /// <summary>
    /// Request to get all existed <see cref="WebLocalDistrict"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllWebLocalDistrictRequest : IQuery<List<WebLocalDistrict>>
    {
        public int? ProvinceId { get; set; }
    }
}
