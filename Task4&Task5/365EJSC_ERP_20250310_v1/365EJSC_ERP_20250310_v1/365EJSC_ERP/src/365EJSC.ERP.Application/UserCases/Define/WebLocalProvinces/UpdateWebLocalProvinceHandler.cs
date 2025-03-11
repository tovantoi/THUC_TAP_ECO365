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
    ///  Handler for <see cref="UpdateWebLocalProvinceRequest"/>/ 
    /// </summary>
    public class UpdateWebLocalProvinceHandler : IRequestHandler<UpdateWebLocalProvinceRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="WebLocalProvince"/>> 
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository;
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalProvinceHandler"/>, inject needed dependency
        /// </summary>
        public UpdateWebLocalProvinceHandler(IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository, IWebLocalSqlRepository webLocalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalProvinceSqlRepository = webLocalProvinceSqlRepository;
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateWebLocalProvinceRequest"/>, find existing <see cref="WebLocalProvince"/> base on id provided in <see cref="UpdateWebLocalProvinceRequest"/>,
        /// update founded <see cref="WebLocalProvince"/> base on data provided in <see cref="UpdateWebLocalProvinceRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateWebLocalProvinceRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateWebLocalProvinceValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find webLocalProvince base on id provided from database, if webLocalProvince was not found, throw not found exception.
            // Need tracking to update webLocalProvince.
            WebLocalProvince webLocalProvince = await webLocalProvinceSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            if (webLocalProvince is null)
            {
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_PROVINCE_ID_NOT_FOUND, WebLocalProvinceConst.MSG_LOCALPROVINCE_ID_NOT_FOUND);
            }

            // Update webLocalProvince base on data provided in UpdateWebLocalProvinceCommand request.
            // Keep webLocalProvince original data if request fields is null
            request.MapTo(webLocalProvince, true);

            var webLocal = await webLocalSqlRepository.IsExistAsync(x => x.Id == request.KeyLocalization);
            if (!webLocal)
            {
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_KEY_LOCAL_NOT_FOUND, WebLocalConst.MSG_KEY_LOCALIZATION_NOT_FOUND);
            }

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark webLocalProvince as Updated state
                webLocalProvinceSqlRepository.Update(webLocalProvince!);

                // Save webLocalProvince to database
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
