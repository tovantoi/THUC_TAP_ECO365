using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to delete company, contain id
    /// </summary>
    public class DeleteCompanyRequest : ICommand
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}