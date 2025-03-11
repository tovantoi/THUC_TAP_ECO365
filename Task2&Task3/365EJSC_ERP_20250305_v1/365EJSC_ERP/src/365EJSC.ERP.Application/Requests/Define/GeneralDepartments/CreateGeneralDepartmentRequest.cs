using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.GeneralDepartments
{
    /// <summary>
    /// Request to create a department, contain de code, de name,  is actived
    /// </summary>
    public class CreateGeneralDepartmentRequest : ICommand
    {
        public string DeCode { get; set; }

        public string? DeName { get; set; }

        public bool? IsActived { get; set; }
      
    }
}
