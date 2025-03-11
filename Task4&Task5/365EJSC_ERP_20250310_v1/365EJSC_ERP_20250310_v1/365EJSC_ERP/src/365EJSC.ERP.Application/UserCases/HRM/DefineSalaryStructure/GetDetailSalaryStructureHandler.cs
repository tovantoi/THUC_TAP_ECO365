using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Handler for <see cref="GetDetailSalaryStructureRequest"/>
    /// </summary>
    public class GetDetailSalaryStructureHandler : IRequestHandler<GetDetailSalaryStructureRequest, Result<Entities.DefineSalaryStructure>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="SalaryStructure"/>>
        /// </summary>
        private readonly ISalaryStructureSqlRepository salaryStructureSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailSalaryStructureHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailSalaryStructureHandler(ISalaryStructureSqlRepository salaryStructureSqlRepository)
        {
            this.salaryStructureSqlRepository = salaryStructureSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailSalaryStructureRequest"/>, get <see cref="SalaryStructure"/> from database with id provided in <see cref="GetDetailSalaryStructureRequest"/>.
        /// Throw not found exception when <see cref="SalaryStructure"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="SalaryStructure"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<Entities.DefineSalaryStructure>> Handle(GetDetailSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailSalaryStructureValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find SalaryStructure by id provided. If SalaryStructure not found will throw NotFoundException
            Entities.DefineSalaryStructure? salaryStructure = await salaryStructureSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (salaryStructure is null) CustomException.ThrowNotFoundException(typeof(Entities.DefineSalaryStructure), MsgCode.ERR_SALARY_STRUCTURE_ID_NOT_FOUND);
            
            return salaryStructure;
        }
    }
}