using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateErpGeneralPositionRequest"/>/ </summary>
    public class CreateErpGeneralPositionHandler : IRequestHandler<CreateErpGeneralPositionRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="ErpGeneralPosition"/>>  /// </summary>
        private readonly IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateErpGeneralPositionHandler"/>, inject needed dependency
        /// </summary>
        public CreateErpGeneralPositionHandler(IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.erpGeneralPositionSqlRepository = erpGeneralPositionSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateErpGeneralPositionRequest"/>, create new <see cref="ErpGeneralPosition"/> base on data <see cref="CreateErpGeneralPositionRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateErpGeneralPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateErpGeneralPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new erpGeneralPosition from request
            ErpGeneralPosition? erpGeneralPosition = request.MapTo<ErpGeneralPosition>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked erpGeneralPosition as Created state
                erpGeneralPositionSqlRepository.Add(erpGeneralPosition);

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
