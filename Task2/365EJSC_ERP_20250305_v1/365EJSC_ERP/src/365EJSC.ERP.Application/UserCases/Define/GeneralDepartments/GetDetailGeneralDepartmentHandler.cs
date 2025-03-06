using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.Validators.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments
{
    /// <summary>
    /// Handler for <see cref="GetDetailGeneralDepartmentRequest"/>
    /// </summary>
    public class GetDetailGeneralDepartmentHandler : IRequestHandler<GetDetailGeneralDepartmentRequest, Result<GeneralDepartment>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="GeneralDepartment"/>
        /// </summary>
        private readonly IGeneralDepartmentSqlRepository generaldepartmentRepository;

        
        /// <summary>
        /// Constructor of <see cref="GetDetailGeneralDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailGeneralDepartmentHandler(IGeneralDepartmentSqlRepository generaldepartmentRepository)
        {
            this.generaldepartmentRepository = generaldepartmentRepository;         
        }

        /// <summary>
        /// Handle <see cref="GetDetailGeneralDepartmentRequest"/>, get <see cref="GeneralDepartment"/> from database with id provided in <see cref="GetDetailGeneralDepartmentRequest"/>.
        /// Throw not found exception when <see cref="GeneralDepartment"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="GeneralDepartment"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<GeneralDepartment>> Handle(GetDetailGeneralDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            GetDetailGeneralDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find general department by id provided. If general department not found will throw NotFoundException
            return await generaldepartmentRepository.FindByIdAsync((int)request.Id, false, cancellationToken);


        }
    }

}
