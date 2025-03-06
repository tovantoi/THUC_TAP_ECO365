using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts
{
    /// <summary>
    /// Request to get existed <see cref="WebLocalDistrict"/> by id from database
    /// </summary>
    public class GetDetailWebLocalDistrictRequest : IQuery<WebLocalDistrict>
    {
        public int? Id { get; set; }
    }
}
