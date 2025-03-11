using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.GeneralDepartments
{
    /// <summary>
    /// Request to update department, containing deparment id and other fields
    /// </summary>
    public class UpdateGeneralDepartmentRequest : ICommand
    {
        public int? Id { get; set; }
        public string DeCode { get; set; }

        public string? DeName { get; set; }

        public bool? IsActived { get; set; }
    }
}
