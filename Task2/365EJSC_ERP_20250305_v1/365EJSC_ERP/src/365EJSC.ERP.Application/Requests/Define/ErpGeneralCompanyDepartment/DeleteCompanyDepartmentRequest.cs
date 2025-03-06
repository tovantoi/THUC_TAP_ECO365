using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Request to delete companydepartment, contain id
    /// </summary>
    public class DeleteCompanyDepartmentRequest : ICommand
    {
        public int Id { get; set; }
    }
}