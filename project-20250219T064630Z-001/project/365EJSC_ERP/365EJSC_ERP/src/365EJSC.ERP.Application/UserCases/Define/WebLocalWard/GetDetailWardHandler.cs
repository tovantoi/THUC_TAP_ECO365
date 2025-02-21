using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWard;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWard
{
    public class GetDetailWardHandler : IRequestHandler<GetDetailWardQuery, Result<WebsiteLocalizationWard>>
    {
        private readonly IWardSqlRepository wardSqlRepository;

        public GetDetailWardHandler(IWardSqlRepository wardSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
        }
        public async Task<Result<WebsiteLocalizationWard>> Handle(GetDetailWardQuery request, CancellationToken cancellationToken)
        {
            GetDetailWardValidator validator = new();
            validator.ValidateAndThrow(request);

            return await wardSqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
        }
    }
}
