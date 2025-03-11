using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.Bank
{
    public class GetAllBankHandler : IRequestHandler<GetAllBankRequest, Result<List<HrmBank>>>
    {
        private readonly IBankSqlRepository bankSqlRepository;

        public GetAllBankHandler(IBankSqlRepository bankSqlRepository)
        {
            this.bankSqlRepository = bankSqlRepository;
        }
        public Task<Result<List<HrmBank>>> Handle(GetAllBankRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<HrmBank>>>(bankSqlRepository.FindAll().ToList());
        }
    }
}
