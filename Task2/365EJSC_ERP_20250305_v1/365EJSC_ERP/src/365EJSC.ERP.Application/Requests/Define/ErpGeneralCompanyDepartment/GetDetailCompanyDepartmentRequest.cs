using _365EJSC.ERP.Contract.Abstractions;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Request to get existed <see cref="Entities.ErpGeneralCompanyDepartment"/> by id from database
    /// </summary>
    public class GetDetailCompanyDepartmentRequest : IQuery<Entities.ErpGeneralCompanyDepartment>
    {
        public int Id { get; set; }
    }
}