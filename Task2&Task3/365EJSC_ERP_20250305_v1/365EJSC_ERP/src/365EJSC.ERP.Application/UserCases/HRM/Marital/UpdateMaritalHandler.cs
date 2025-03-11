using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.Marital
{
    /// <summary>
    ///  Handler for <see cref="UpdateMaritalRequest"/>/ 
    /// </summary>
    public class UpdateMaritalHandler : IRequestHandler<UpdateMaritalRequest, Result<object>>
    {
        private readonly IMaritalSqlRepository hrmMaritalSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;
        public UpdateMaritalHandler(IMaritalSqlRepository hrmMaritalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.hrmMaritalSqlRepository = hrmMaritalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }
        /// <summary>
        /// Handle <see cref="UpdateMaritalRequest"/>, find existing <see cref="HrmMaritals"/> base on id provided in <see cref="UpdateMaritalRequest"/>,
        /// update founded <see cref="HrmMaritals"/> base on data provided in <see cref="UpdateMaritalRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateMaritalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateMaritalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find marital base on id provided from database, if marital was not found, throw not found exception.
            // Need tracking to update Local.
            HrmMarital? marital = await hrmMaritalSqlRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
            if (marital is null) CustomException.ThrowNotFoundException(typeof(HrmMarital), MsgCode.ERR_MARITAL_ID_NOT_FOUND);

            // Update marital base on data provided in UpdateHrmMaritalRequest.
            // Keep marital original data if request fields is null
            request.MapTo(marital, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark marital as Updated state
                hrmMaritalSqlRepository.Update(marital!);

                // Save marital to database
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
