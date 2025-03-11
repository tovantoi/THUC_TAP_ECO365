using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Handler for <see cref="GetAllSalaryStructureRequest"/>
    /// </summary>
    public class GetAllSalaryStructureHandler : IRequestHandler<GetAllSalaryStructureRequest, Result<List<Entities.DefineSalaryStructure>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="SalaryStructure"/>>
        /// </summary>
        private readonly ISalaryStructureSqlRepository salaryStructureSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllSalaryStructureHandler"/>, inject needed dependency
        /// </summary>
        public GetAllSalaryStructureHandler(ISalaryStructureSqlRepository salaryStructureSqlRepository)
        {
            this.salaryStructureSqlRepository = salaryStructureSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllSalaryStructureRequest"/>, get all SalaryStructure in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="SalaryStructure"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<Entities.DefineSalaryStructure>>> Handle(GetAllSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<Entities.DefineSalaryStructure>>>(salaryStructureSqlRepository.FindAll().ToList());
        }
    }
}