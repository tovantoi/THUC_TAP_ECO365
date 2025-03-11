using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Handler for <see cref="DeleteSalaryStructureRequest"/>
    /// </summary>
    public class DeleteSalaryStructureHandler : IRequestHandler<DeleteSalaryStructureRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="SalaryStructure"/>>
        /// </summary>
        private readonly ISalaryStructureSqlRepository salaryStructureSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteSalaryStructureHandler"/>, inject needed dependency
        /// </summary>
        public DeleteSalaryStructureHandler(ISalaryStructureSqlRepository salaryStructureSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.salaryStructureSqlRepository = salaryStructureSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteSalaryStructureRequest"/>, find existing <see cref="SalaryStructure"/> base on id provided in <see cref="DeleteSalaryStructureRequest"/>,
        /// delete founded <see cref="SalaryStructure"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteSalaryStructureValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find SalaryStructure base on id provided from database, if SalaryStructure was not found, throw not found exception.
            // Need tracking to delete SalaryStructure.
            Entities.DefineSalaryStructure salaryStructure = await salaryStructureSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (salaryStructure is null) CustomException.ThrowNotFoundException(typeof(Entities.DefineSalaryStructure), MsgCode.ERR_SALARY_STRUCTURE_ID_NOT_FOUND);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked SalaryStructure as Deleted state
                salaryStructureSqlRepository.Remove(salaryStructure);

                // Save changes to database
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