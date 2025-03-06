using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.GeneralDepartments
{
    public class DeleteGeneralDepartmentRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
