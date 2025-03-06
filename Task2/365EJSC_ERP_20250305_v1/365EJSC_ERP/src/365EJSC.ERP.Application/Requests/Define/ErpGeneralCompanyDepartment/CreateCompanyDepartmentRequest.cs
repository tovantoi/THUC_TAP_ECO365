using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Request to create companydepartment, contain companyid and departmentid
    /// </summary>
    public class CreateCompanyDepartmentRequest : ICommand
    {
        public int CompanyId { get; set; }
        public ICollection<int> DepartmentIds { get; set; } = new List<int>();
    }
}