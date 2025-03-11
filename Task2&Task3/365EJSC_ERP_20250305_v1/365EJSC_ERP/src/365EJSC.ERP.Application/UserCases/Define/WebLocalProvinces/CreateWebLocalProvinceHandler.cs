using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
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

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateWebLocalProvinceRequest"/>/ </summary>
    public class CreateWebLocalProvinceHandler : IRequestHandler<CreateWebLocalProvinceRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocalProvince"/>>  /// </summary>
        private readonly IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository;
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateWebLocalProvinceHandler"/>, inject needed dependency
        /// </summary>
        public CreateWebLocalProvinceHandler(IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository, IWebLocalSqlRepository webLocalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalProvinceSqlRepository = webLocalProvinceSqlRepository;
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateWebLocalProvinceRequest"/>, create new <see cref="WebLocalProvince"/> base on data <see cref="CreateWebLocalProvinceRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateWebLocalProvinceRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateWebLocalProvinceValidator validator = new();
            validator.ValidateAndThrow(request);

            var webLocal = await webLocalSqlRepository.IsExistAsync(x => x.Id == request.KeyLocalization);
            if (!webLocal)
            {
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_KEY_LOCAL_NOT_FOUND, WebLocalConst.MSG_KEY_LOCALIZATION_NOT_FOUND);
            }

            // Create new webLocalProvince from request
            WebLocalProvince? webLocalProvince = request.MapTo<WebLocalProvince>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked webLocalProvince as Created state
                webLocalProvinceSqlRepository.Add(webLocalProvince);

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
