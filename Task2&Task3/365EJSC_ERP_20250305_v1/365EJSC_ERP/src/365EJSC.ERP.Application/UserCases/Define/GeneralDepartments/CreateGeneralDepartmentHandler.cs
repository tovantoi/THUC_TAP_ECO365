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
    /// Handler for <see cref="CreateGeneralDepartmentRequest"/>
    /// </summary>
    public class CreateGeneralDepartmentHandler : IRequestHandler<CreateGeneralDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Department"/>
        /// </summary>
        private readonly IGeneralDepartmentSqlRepository DepartmentSqlRepository;
     
        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateGeneralDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public CreateGeneralDepartmentHandler(IGeneralDepartmentSqlRepository DepartmentSqlRepository,
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.DepartmentSqlRepository = DepartmentSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateGeneralDepartmentRequest"/>, create new <see cref="Department"/> based on data <see cref="CreateGeneralDepartmentRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateGeneralDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateGeneralDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);


            // Create new Department from request
            GeneralDepartment? Department = request.MapTo<GeneralDepartment>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Department as Created state
                DepartmentSqlRepository.Add(Department);

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
