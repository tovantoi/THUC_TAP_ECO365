using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO;
using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to get all existed <see cref="Domain.Entities.Define.ErpGeneralCompany"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllCompanyRequest : IQuery<List<CompanyDTO>>
    {
    }
}