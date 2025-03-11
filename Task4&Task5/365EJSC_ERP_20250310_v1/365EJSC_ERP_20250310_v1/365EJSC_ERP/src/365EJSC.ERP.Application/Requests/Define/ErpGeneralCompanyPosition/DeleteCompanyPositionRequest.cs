using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Request to delete companyPosition, contain id
    /// </summary>
    public class DeleteCompanyPositionRequest : ICommand
    {
        public int Id { get; set; }
    }
}