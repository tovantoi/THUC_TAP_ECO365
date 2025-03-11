using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Request to create companyposition, contain companyid and positionid
    /// </summary>
    public class CreateCompanyPositionRequest : ICommand
    {
        public int CompanyId { get; set; }
        public ICollection<int> PositionIds { get; set; } = new List<int>();
    }
}