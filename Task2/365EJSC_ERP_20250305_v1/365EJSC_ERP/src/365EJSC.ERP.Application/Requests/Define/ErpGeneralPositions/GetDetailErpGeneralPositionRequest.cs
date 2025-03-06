using _365EJSC.ERP.Contract.Abstractions;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions
{
    /// <summary>
    /// Request to get existed <see cref="ErpGeneralPosition"/> by id from database
    /// </summary>
    public record GetDetailErpGeneralPositionRequest : IQuery<ErpGeneralPosition>
    {
        public int? Id { get; set; }
    }
}
