using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.Validators.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.Bank
{
    /// <summary>
    ///  Handler for <see cref="UpdateBankRequest"/>/ 
    /// </summary>
    public class UpdateBankHandler : IRequestHandler<UpdateBankRequest, Result<object>>
    {
        private readonly IBankSqlRepository bankSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public UpdateBankHandler(IBankSqlRepository bankSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.bankSqlRepository = bankSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateBankRequest"/>, find existing <see cref="HrmMaritals"/> base on id provided in <see cref="UpdateBankRequest"/>,
        /// update founded <see cref="HrmMaritals"/> base on data provided in <see cref="UpdateBankRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateBankRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateBankValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find bank base on id provided from database, if bank was not found, throw not found exception.
            // Need tracking to update Local.
            HrmBank? bank = await bankSqlRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
            if (bank is null) CustomException.ThrowNotFoundException(typeof(HrmBank), MsgCode.ERR_BANK_ID_NOT_FOUND);

            // Update bank base on data provided in UpdateHrmBankRequest.
            // Keep bank original data if request fields is null
            request.MapTo(bank, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark bank as Updated state
                bankSqlRepository.Update(bank!);

                // Save bank to database
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
