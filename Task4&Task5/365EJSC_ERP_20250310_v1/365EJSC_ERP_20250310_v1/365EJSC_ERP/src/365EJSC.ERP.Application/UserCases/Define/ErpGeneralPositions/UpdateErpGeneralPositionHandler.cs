using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions
{
    /// <summary>
    ///  Handler for <see cref="UpdateErpGeneralPositionRequest"/>/ 
    /// </summary>
    public class UpdateErpGeneralPositionHandler : IRequestHandler<UpdateErpGeneralPositionRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="ErpGeneralPosition"/>> 
        /// </summary>
        private readonly IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateErpGeneralPositionHandler"/>, inject needed dependency
        /// </summary>
        public UpdateErpGeneralPositionHandler(IErpGeneralPositionSqlRepository erpGeneralPositionSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.erpGeneralPositionSqlRepository = erpGeneralPositionSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateErpGeneralPositionRequest"/>, find existing <see cref="ErpGeneralPosition"/> base on id provided in <see cref="UpdateErpGeneralPositionRequest"/>,
        /// update founded <see cref="ErpGeneralPosition"/> base on data provided in <see cref="UpdateErpGeneralPositionRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateErpGeneralPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateErpGeneralPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find erpGeneralPosition base on id provided from database, if erpGeneralPosition was not found, throw not found exception.
            // Need tracking to update erpGeneralPosition.
            ErpGeneralPosition erpGeneralPosition = await erpGeneralPositionSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            if (erpGeneralPosition is null)
            {
                CustomException.ThrowNotFoundException(typeof(ErpGeneralPosition), MsgCode.ERR_POSITION_ID_NOT_FOUND, ErpGeneralPositionConst.MSG_GENERAL_POSITION_ID_NOT_FOUND);
            }

            // Update erpGeneralPosition base on data provided in UpdateErpGeneralPositionRequest request.
            // Keep erpGeneralPosition original data if request fields is null
            request.MapTo(erpGeneralPosition, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark erpGeneralPosition as Updated state
                erpGeneralPositionSqlRepository.Update(erpGeneralPosition!);

                // Save erpGeneralPosition to database
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
