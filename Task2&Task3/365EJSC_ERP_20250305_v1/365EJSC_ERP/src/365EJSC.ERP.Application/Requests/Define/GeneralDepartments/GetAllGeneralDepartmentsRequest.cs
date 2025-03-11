using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.GeneralDepartments
{
    /// <summary>
    /// Request to get all existed <see cref="GeneralDepartment"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllGeneralDepartmentsRequest : IQuery<List<GeneralDepartment>>
    {
    }
}
