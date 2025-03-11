using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
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
    /// Handler for <see cref="DeleteWebLocalRequests"/>
    /// </summary>
    public class DeleteWebLocalHandler : IRequestHandler<DeleteWebLocalRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocals"/>>  /// </summary>
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalHandler"/>, inject needed dependency
        /// </summary>
        public DeleteWebLocalHandler(IWebLocalSqlRepository webLocalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteWebLocalRequests"/>, find existing <see cref="WebLocals"/> base on id provided in <see cref="DeleteWebLocalRequests"/>,
        /// delete founded <see cref="WebLocals"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteWebLocalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWebLocalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            WebLocals? webLocal = await webLocalSqlRepository.FindSingleAsync(x => x.Id == request.Id && x.IsActived == true, true, cancellationToken);
            if (webLocal is null) CustomException.ThrowNotFoundException(typeof(WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND);
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                webLocal.IsActived = false;
                // Marked sample as Deleted state
                webLocalSqlRepository.Update(webLocal);

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