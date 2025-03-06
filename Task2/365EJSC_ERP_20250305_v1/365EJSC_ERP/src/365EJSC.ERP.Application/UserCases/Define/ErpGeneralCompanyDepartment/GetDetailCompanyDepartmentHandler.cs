using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Handler for <see cref="GetDetailCompanyDepartmentRequest"/>
    /// </summary>
    public class GetDetailCompanyDepartmentHandler : IRequestHandler<GetDetailCompanyDepartmentRequest, Result<Entities.ErpGeneralCompanyDepartment>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompanyDepartment"/>>
        /// </summary>
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;

        // <summary>
        /// Constructor of <see cref="GetDetailCompanyDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailCompanyDepartmentHandler(ICompanyDepartmentSqlRepository companyDepartmentSqlRepository)
        {
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailCompanyQuery"/>, get <see cref="Entities.ErpGeneralCompanyDepartment"/> from database with id provided in <see cref="GetDetailCompanyDepartmentRequest"/>.
        /// Throw not found exception when <see cref="Entities.ErpGeneralCompanyDepartment"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Entities.ErpGeneralCompanyDepartment"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Entities.ErpGeneralCompanyDepartment>> Handle(GetDetailCompanyDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailCompanyDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company by id provided. If CompanyDepartment not found will throw NotFoundException
            Entities.ErpGeneralCompanyDepartment companyDepartment = await companyDepartmentSqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
            if (companyDepartment is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyDepartment), MsgCode.ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND);
            return companyDepartment;
        }
    }
}