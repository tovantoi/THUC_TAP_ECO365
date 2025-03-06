using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocal
{    /// <summary>
     /// Hand/// Handler for <see cref="CreateWebLocalRequests"/>/ </summary>
    public class CreateWebLocalHandler : IRequestHandler<CreateWebLocalRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocals"/>>  /// </summary>
        private readonly IWebLocalSqlRepository webLocalSqlRepository;
        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateWebLocalHandler"/>, inject needed dependency
        /// </summary>
        public CreateWebLocalHandler(IWebLocalSqlRepository webLocalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }
        /// <summary>
        /// Handle <see cref="CreateWebLocalRequests"/>, create new <see cref="WebLocal"/> base on data <see cref="CreateSampleCommand"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateWebLocalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateWebLocalValidator validator = new();
            validator.ValidateAndThrow(request);
			Entities.WebLocals? existingLocal = await webLocalSqlRepository.FindByIdAsync(request.Id, false, cancellationToken);
			if (existingLocal is not null) CustomException.ThrowConflictException(MsgCode.ERR_KEY_LOCAL_EXISTED, WebLocalConst.MSG_KEY_LOCALIZATION_EXISTED);
			// Create new webLocal from request
			WebLocals? webLocal = request.MapTo<WebLocals>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked sample as Created state
                webLocalSqlRepository.Add(webLocal);

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