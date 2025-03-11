using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Request to update companyPosition, contain id, companyid and Positionid
    /// </summary>
    public class UpdateCompanyPositionRequest : ICommand
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int? PositionId { get; set; }
    }
}