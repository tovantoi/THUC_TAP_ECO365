using _365EJSC.ERP.Contract.Abstractions;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Request to get all existed <see cref="Entities.DefineSalaryStructure"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllSalaryStructureRequest : IQuery<List<Entities.DefineSalaryStructure>>
    {
    }
}