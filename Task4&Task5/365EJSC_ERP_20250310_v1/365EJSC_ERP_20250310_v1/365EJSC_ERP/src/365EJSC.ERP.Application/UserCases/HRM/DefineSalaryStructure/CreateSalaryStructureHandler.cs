using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateSalaryStructureRequest"/>/ </summary>
    public class CreateSalaryStructureHandler : IRequestHandler<CreateSalaryStructureRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="SalaryStructure"/>>  /// </summary>
        private readonly ISalaryStructureSqlRepository salaryStructureSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateSalaryStructureHandler"/>, inject needed dependency
        /// </summary>
        public CreateSalaryStructureHandler(ISalaryStructureSqlRepository salaryStructureSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.salaryStructureSqlRepository = salaryStructureSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateSalaryStructureRequest"/>, create new <see cref="SalaryStructure"/> base on data <see cref="CreateSalaryStructureRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateSalaryStructureValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new SalaryStructure from request
            Entities.DefineSalaryStructure? salaryStructure = request.MapTo<Entities.DefineSalaryStructure>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked SalaryStructure as Created state
                salaryStructureSqlRepository.Add(salaryStructure);

                // Save data to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happened, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}