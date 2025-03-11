using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Handler for <see cref="GetDetailCompanyPositionRequest"/>
    /// </summary>
    public class GetDetailCompanyPositionHandler : IRequestHandler<GetDetailCompanyPositionRequest, Result<Entities.ErpGeneralCompanyPosition>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompanyPosition"/>>
        /// </summary>
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        // <summary>
        /// Constructor of <see cref="GetDetailCompanyPositionHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailCompanyPositionHandler(ICompanyPositionSqlRepository companyPositionSqlRepository)
        {
            this.companyPositionSqlRepository = companyPositionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailCompanyQuery"/>, get <see cref="Entities.ErpGeneralCompanyPosition"/> from database with id provided in <see cref="GetDetailCompanyPositionRequest"/>.
        /// Throw not found exception when <see cref="Entities.ErpGeneralCompanyPosition"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Entities.ErpGeneralCompanyPosition"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Entities.ErpGeneralCompanyPosition>> Handle(GetDetailCompanyPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailCompanyPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company by id provided. If CompanyPosition not found will throw NotFoundException
            Entities.ErpGeneralCompanyPosition companyPosition = await companyPositionSqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
            if (companyPosition is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyPosition), MsgCode.ERR_COMPANY_POSITION_ID_NOT_FOUND);
            return companyPosition;
        }
    }
}