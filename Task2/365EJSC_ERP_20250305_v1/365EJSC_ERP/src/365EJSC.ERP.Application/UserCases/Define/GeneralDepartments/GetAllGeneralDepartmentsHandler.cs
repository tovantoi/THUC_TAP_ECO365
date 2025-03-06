using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments
{
    /// <summary>
    /// Handler for <see cref="GetAllGeneralDepartmentRequest"/>
    /// </summary>
    public class GetAllGeneralDepartmentsHandler : IRequestHandler<GetAllGeneralDepartmentsRequest, Result<List<GeneralDepartment>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="GeneralDepartment"/>
        /// </summary>
        private readonly IGeneralDepartmentSqlRepository generalDepartmentRepository;

       
        /// <summary>
        /// Constructor of <see cref="GetAllGeneralDepartmentsHandler"/>, inject needed dependency
        /// </summary>
        public GetAllGeneralDepartmentsHandler(IGeneralDepartmentSqlRepository generalDepartmentRepository)
        {
            this.generalDepartmentRepository = generalDepartmentRepository;          
        }

        /// <summary>
        /// Handle <see cref="GetAllGeneralDepartmentRequest"/>, get all departments in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="GeneralDepartment"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<GeneralDepartment>>> Handle(GetAllGeneralDepartmentsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<GeneralDepartment>>>(generalDepartmentRepository.FindAll().ToList());
        }
    }
}
