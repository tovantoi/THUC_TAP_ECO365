using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions
{
    /// <summary>
    /// Request to delete erpGeneralPosition, contain position id
    /// </summary>
    public record UpdateErpGeneralPositionRequest : ICommand
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
