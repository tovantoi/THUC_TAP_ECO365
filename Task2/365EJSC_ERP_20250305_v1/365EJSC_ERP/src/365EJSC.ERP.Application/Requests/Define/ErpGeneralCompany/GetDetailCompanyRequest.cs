using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to get existed <see cref="Domain.Entities.Define.ErpGeneralCompany"/> by id from database
    /// </summary>
    public class GetDetailCompanyRequest : IQuery<Domain.Entities.Define.ErpGeneralCompany>
    {
        public int? Id { get; set; }
    }
}