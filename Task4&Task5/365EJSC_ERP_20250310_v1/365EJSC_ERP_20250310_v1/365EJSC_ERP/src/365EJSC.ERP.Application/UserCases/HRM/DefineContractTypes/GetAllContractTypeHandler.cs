using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes
{
    public class GetAllContractTypeHandler : IRequestHandler<GetAllContractTypeRequest, Result<List<DefineContractType>>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="DefineContractType"/>>  /// </summary>
        private readonly IContractTypeSqlRepository contractTypeSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllContractTypeHandler"/>, inject needed dependency
        /// </summary>
        public GetAllContractTypeHandler(IContractTypeSqlRepository contractTypeSqlRepository)
        {
            this.contractTypeSqlRepository = contractTypeSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllContractTypeRequest"/>, get all DefineContractType in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="DefineContractType"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<DefineContractType>>> Handle(GetAllContractTypeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<DefineContractType>>>(contractTypeSqlRepository.FindAll().ToList());
        }
    }
}
