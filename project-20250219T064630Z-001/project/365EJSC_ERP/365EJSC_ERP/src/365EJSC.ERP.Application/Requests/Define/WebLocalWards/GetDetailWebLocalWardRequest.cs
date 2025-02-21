using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    public class GetDetailWebLocalWardRequest : IRequest<Result<WebLocalWard>>
    {
        public int? Id { get; set; }
    }

}
