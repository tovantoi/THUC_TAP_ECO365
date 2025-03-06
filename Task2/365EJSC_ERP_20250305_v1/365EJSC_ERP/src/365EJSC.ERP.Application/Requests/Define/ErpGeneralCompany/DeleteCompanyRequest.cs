using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to delete company, contain id
    /// </summary>
    public class DeleteCompanyRequest : ICommand
    {
        public int? Id { get; set; }
    }
}