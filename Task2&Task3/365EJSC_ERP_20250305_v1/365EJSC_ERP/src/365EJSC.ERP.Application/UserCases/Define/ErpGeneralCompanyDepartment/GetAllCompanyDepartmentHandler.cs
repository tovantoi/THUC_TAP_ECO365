using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Handler for <see cref="GetAllCompanyDepartmentRequest"/>
    /// </summary>
    public class GetAllCompanyDepartmentHandler : IRequestHandler<GetAllCompanyDepartmentRequest, Result<List<Entities.ErpGeneralCompanyDepartment>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompanyDepartment"/>>
        /// </summary>
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;

        // <summary>
        /// Constructor of <see cref="GetAllCompanyDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public GetAllCompanyDepartmentHandler(ICompanyDepartmentSqlRepository companyDepartmentSqlRepository)
        {
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllCompanyDepartmentRequest"/>, get all Companys in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="Entities.ErpGeneralCompanyDepartment"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Entities.ErpGeneralCompanyDepartment>>> Handle(GetAllCompanyDepartmentRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Entities.ErpGeneralCompanyDepartment>>>(companyDepartmentSqlRepository.FindAll().ToList());
        }
    }
}