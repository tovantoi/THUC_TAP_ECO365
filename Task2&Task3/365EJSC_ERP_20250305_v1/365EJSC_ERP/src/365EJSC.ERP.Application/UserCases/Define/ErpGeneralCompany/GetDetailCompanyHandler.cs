using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    /// <summary>
    /// Handler for <see cref="GetDetailCompanyQuery"/>
    /// </summary>
    public class GetDetailCompanyHandler : IRequestHandler<GetDetailCompanyRequest, Result<Entities.ErpGeneralCompany>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompany"/>>
        /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;

        // <summary>
        /// Constructor of <see cref="GetAllCompanyHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailCompanyHandler(ICompanySqlRepository companySqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailCompanyQuery"/>, get <see cref="Entities.ErpGeneralCompany"/> from database with id provided in <see cref="GetDetailCompanyQuery"/>.
        /// Throw not found exception when <see cref="Entities.ErpGeneralCompany"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Entities.ErpGeneralCompany"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Entities.ErpGeneralCompany>> Handle(GetDetailCompanyRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailCompanyValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company by id provided. If Company not found will throw NotFoundException
            Entities.ErpGeneralCompany company = await companySqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);
            return company;
        }
    }
}