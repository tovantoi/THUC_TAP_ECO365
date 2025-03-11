using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Handler for <see cref="GetAllCompanyPositionRequest"/>
    /// </summary>
    public class GetAllCompanyPositionHandler : IRequestHandler<GetAllCompanyPositionRequest, Result<List<Entities.ErpGeneralCompanyPosition>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompanyPosition"/>>
        /// </summary>
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        // <summary>
        /// Constructor of <see cref="GetAllCompanyPositionHandler"/>, inject needed dependency
        /// </summary>
        public GetAllCompanyPositionHandler(ICompanyPositionSqlRepository companyPositionSqlRepository)
        {
            this.companyPositionSqlRepository = companyPositionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllCompanyPositionRequest"/>, get all Companys in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="Entities.ErpGeneralCompanyPosition"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Entities.ErpGeneralCompanyPosition>>> Handle(GetAllCompanyPositionRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Entities.ErpGeneralCompanyPosition>>>(companyPositionSqlRepository.FindAll().ToList());
        }
    }
}