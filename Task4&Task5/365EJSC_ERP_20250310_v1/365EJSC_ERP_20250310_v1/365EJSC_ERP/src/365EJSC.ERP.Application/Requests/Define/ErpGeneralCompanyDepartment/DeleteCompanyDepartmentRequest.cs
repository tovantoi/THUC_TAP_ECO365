using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Request to delete companydepartment, contain id
    /// </summary>
    public class DeleteCompanyDepartmentRequest : ICommand
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}