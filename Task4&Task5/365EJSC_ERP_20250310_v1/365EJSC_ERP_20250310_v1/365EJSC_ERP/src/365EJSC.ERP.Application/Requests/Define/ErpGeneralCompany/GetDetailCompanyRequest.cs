using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO;
using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to get existed <see cref="Domain.Entities.Define.ErpGeneralCompany"/> by id from database
    /// </summary>
    public class GetDetailCompanyRequest : IQuery<CompanyDTO>
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}