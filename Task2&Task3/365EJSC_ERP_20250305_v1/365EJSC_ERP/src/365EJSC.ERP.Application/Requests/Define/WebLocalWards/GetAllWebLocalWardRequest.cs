using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    /// <summary>
    /// Request to get all existed <see cref="WebLocalWard"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllWebLocalWardRequest : IRequest<Result<List<WebLocalWard>>>
    {
        public int? DistrictId { get; set; }
    }
}
