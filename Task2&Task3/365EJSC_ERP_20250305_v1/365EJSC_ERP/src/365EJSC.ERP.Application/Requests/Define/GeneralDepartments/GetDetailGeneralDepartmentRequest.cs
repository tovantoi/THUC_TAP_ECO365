using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.GeneralDepartments
{
    /// <summary>
    /// Request to get existed <see cref="GeneralDepartment"/> by id from database
    /// </summary>
    public class GetDetailGeneralDepartmentRequest : IQuery<GeneralDepartment>
    {
        public int? Id { get; set; }
    }
}
