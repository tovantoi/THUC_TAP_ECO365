using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.Marital
{
    public class GetAllMaritalHandler : IRequestHandler<GetAllMaritalRequest, Result<List<HrmMarital>>>
    {
        private readonly IMaritalSqlRepository hrmMaritalSqlRepository;

        public GetAllMaritalHandler(IMaritalSqlRepository hrmMaritalSqlRepository)
        {
            this.hrmMaritalSqlRepository = hrmMaritalSqlRepository;
        }

        public Task<Result<List<HrmMarital>>> Handle(GetAllMaritalRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<HrmMarital>>>(hrmMaritalSqlRepository.FindAll().ToList());
        }
    }
}
