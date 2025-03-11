using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.Validators.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments
{
    /// <summary>
    /// Handler for <see cref="UpdateGeneralDepartmentRequest"/>
    /// </summary>
    public class UpdateGeneralDepartmentHandler : IRequestHandler<UpdateGeneralDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="GeneralDepartment"/>
        /// </summary>
        private readonly IGeneralDepartmentSqlRepository generalDepartmentRepository;


        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateGeneralDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public UpdateGeneralDepartmentHandler(IGeneralDepartmentSqlRepository GeneralDepartmentRepository,                                           
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.generalDepartmentRepository = GeneralDepartmentRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateGeneralDepartmentRequest"/>, find existing <see cref="GeneralDepartment"/> based on id provided in <see cref="UpdateGeneralDepartmentRequest"/>,
        /// update the found <see cref="GeneralDepartment"/> based on data provided in <see cref="UpdateGeneralDepartmentRequest"/> and save to the database.
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(UpdateGeneralDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateGeneralDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find GeneralDepartment based on id provided from the database, if GeneralDepartment is not found, throw not found exception.
            // Need tracking to update the GeneralDepartment.
            GeneralDepartment GeneralDepartment = await generalDepartmentRepository.FindByIdAsync((int)request.Id, true, cancellationToken);          

            // Update GeneralDepartment based on data provided in UpdateGeneralDepartmentRequest request.
            // Keep GeneralDepartment original data if request fields are null
            request.MapTo(GeneralDepartment, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark GeneralDepartment as Updated state
                generalDepartmentRepository.Update(GeneralDepartment!);

                // Save GeneralDepartment to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happens, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
