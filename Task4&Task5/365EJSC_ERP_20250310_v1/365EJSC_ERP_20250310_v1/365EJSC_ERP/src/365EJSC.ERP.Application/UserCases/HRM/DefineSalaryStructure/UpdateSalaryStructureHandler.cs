using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
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
    ///  Handler for <see cref="UpdateSalaryStructureRequest"/>/ 
    /// </summary>
    public class UpdateSalaryStructureHandler : IRequestHandler<UpdateSalaryStructureRequest, Result<object>>
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
        /// Constructor of <see cref="UpdateSalaryStructureHandler"/>, inject needed dependency
        /// </summary>
        public UpdateSalaryStructureHandler(ISalaryStructureSqlRepository salaryStructureSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.salaryStructureSqlRepository = salaryStructureSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateSalaryStructureRequest"/>, find existing <see cref="SalaryStructure"/> base on id provided in <see cref="UpdateSalaryStructureRequest"/>,
        /// update founded <see cref="SalaryStructure"/> base on data provided in <see cref="UpdateSalaryStructureRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateSalaryStructureValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find SalaryStructure base on id provided from database, if SalaryStructure was not found, throw not found exception.
            // Need tracking to update SalaryStructure.
            Entities.DefineSalaryStructure? salaryStructure = await salaryStructureSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (salaryStructure is null) CustomException.ThrowNotFoundException(typeof(Entities.DefineSalaryStructure), MsgCode.ERR_SALARY_STRUCTURE_ID_NOT_FOUND);

            // Update SalaryStructure base on data provided in UpdateSalaryStructureRequest request.
            // Keep SalaryStructure original data if request fields is null
            request.MapTo(salaryStructure, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark SalaryStructure as Updated state
                salaryStructureSqlRepository.Update(salaryStructure!);

                // Save SalaryStructure to database
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