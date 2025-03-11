using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.Validators.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes
{
    public class CreateContractTypeHandler : IRequestHandler<CreateContractTypeRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="DefineContractType"/>>  /// </summary>
        private readonly IContractTypeSqlRepository contractTypeSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateContractTypeHandler"/>, inject needed dependency
        /// </summary>
        public CreateContractTypeHandler(IContractTypeSqlRepository contractTypeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.contractTypeSqlRepository = contractTypeSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateContractTypeRequest"/>, create new <see cref="DefineContractType"/> base on data <see cref="CreateContractTypeRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateContractTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateContractTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new erpGeneralPosition from request
            DefineContractType? defineContractType = request.MapTo<DefineContractType>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked erpGeneralPosition as Created state
                contractTypeSqlRepository.Add(defineContractType);

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
