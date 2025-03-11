using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions
{
    /// <summary>
    /// Handler for <see cref="GetAllErpGeneralPositionRequest"/>
    /// </summary>
    public class GetAllErpGeneralPositionHandler : IRequestHandler<GetAllErpGeneralPositionRequest, Result<List<ErpGeneralPosition>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="ErpGeneralPosition"/>>
        /// </summary>
        private readonly IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllErpGeneralPositionHandler"/>, inject needed dependency
        /// </summary>
        public GetAllErpGeneralPositionHandler(IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository)
        {
            this.erpGeneralPositionSqlRepository = erpGeneralPositionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllErpGeneralPositionRequest"/>, get all ErpGeneralPosition in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="ErpGeneralPosition"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<ErpGeneralPosition>>> Handle(GetAllErpGeneralPositionRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<ErpGeneralPosition>>>(erpGeneralPositionSqlRepository.FindAll().ToList());
        }
    }
}
