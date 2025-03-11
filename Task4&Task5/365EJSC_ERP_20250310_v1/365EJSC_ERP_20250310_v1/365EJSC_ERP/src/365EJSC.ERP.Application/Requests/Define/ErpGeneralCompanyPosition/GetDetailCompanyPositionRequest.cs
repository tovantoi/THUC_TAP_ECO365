using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Request to get existed <see cref="Entities.ErpGeneralCompanyPosition"/> by id from database
    /// </summary>  
    public class GetDetailCompanyPositionRequest : IQuery<Entities.ErpGeneralCompanyPosition>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}