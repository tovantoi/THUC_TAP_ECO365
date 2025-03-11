using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Request to update companydepartment, contain id, companyid and departmentid
    /// </summary>
    public class UpdateCompanyDepartmentRequest : ICommand
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
    }
}