using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions
{
    /// <summary>
    /// Request to create erpGeneralPosition, contain code and name
    /// </summary>
    public record CreateErpGeneralPositionRequest : ICommand
    {
        public string? Code { get; set; }
        public string Name { get; set; }
    }
}
