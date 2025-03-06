using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocal
{   /// <summary>
    ///  Handler for <see cref="UpdateWebLocalRequests"/>/ 
    /// </summary>
    public class UpdateWebLocalHandler : IRequestHandler<UpdateWebLocalRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocals"/>>  /// </summary>
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalHandler"/>, inject needed dependency
        /// </summary>
        public UpdateWebLocalHandler(IWebLocalSqlRepository webLocalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateWebLocalRequests"/>, find existing <see cref="WebLocals"/> base on id provided in <see cref="UpdateWebLocalRequests"/>,
        /// update founded <see cref="WebLocals"/> base on data provided in <see cref="UpdateWebLocalRequests"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateWebLocalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateWebLocalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Local base on id provided from database, if Local was not found, throw not found exception.
            // Need tracking to update Local.
            WebLocals? webLocal = await webLocalSqlRepository.FindByIdAsync(request.Id, true, cancellationToken);
            if (webLocal is null) CustomException.ThrowNotFoundException(typeof(WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND);

            // Update Local base on data provided in UpdateLocalCommand request.
            // Keep Local original data if request fields is null
            request.MapTo(webLocal, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Local as Updated state
                webLocalSqlRepository.Update(webLocal!);

                // Save Local to database
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