using _365EJSC.ERP.Contract.Abstractions;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Request to get all existed <see cref="Entities.ErpGeneralCompanyPosition"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllCompanyPositionRequest : IQuery<List<Entities.ErpGeneralCompanyPosition>>
    {
    }
}