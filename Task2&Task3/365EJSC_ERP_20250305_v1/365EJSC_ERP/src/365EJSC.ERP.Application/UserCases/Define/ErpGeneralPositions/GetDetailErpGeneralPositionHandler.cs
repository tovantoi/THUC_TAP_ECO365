using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions
{
    /// <summary>
    /// Handler for <see cref="GetDetailErpGeneralPositionRequest"/>
    /// </summary>
    public class GetDetailErpGeneralPositionHandler : IRequestHandler<GetDetailErpGeneralPositionRequest, Result<ErpGeneralPosition>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="ErpGeneralPosition"/>>
        /// </summary>
        private readonly IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailErpGeneralPositionHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailErpGeneralPositionHandler(IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository)
        {
            this.erpGeneralPositionSqlRepository = erpGeneralPositionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailErpGeneralPositionRequest"/>, get <see cref="ErpGeneralPosition"/> from database with id provided in <see cref="GetDetailErpGeneralPositionRequest"/>.
        /// Throw not found exception when <see cref="ErpGeneralPosition"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="ErpGeneralPosition"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<ErpGeneralPosition>> Handle(GetDetailErpGeneralPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailErpGeneralPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find erpGeneralPosition by id provided. If erpGeneralPosition not found will throw NotFoundException
            return await erpGeneralPositionSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
        }
    }
}
