using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces
{
    /// <summary>
    /// Handler for <see cref="DeleteWebLocalProvinceRequest"/>
    /// </summary>
    public class DeleteWebLocalProvinceHandler : IRequestHandler<DeleteWebLocalProvinceRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="WebLocalProvince"/>>
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository;
        private readonly IWebLocalDistrictSqlRepository webLocalDistrictSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalProvinceHandler"/>, inject needed dependency
        /// </summary>
        public DeleteWebLocalProvinceHandler(IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository, IWebLocalDistrictSqlRepository webLocalDistrictSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.webLocalProvinceSqlRepository = webLocalProvinceSqlRepository;
            this.webLocalDistrictSqlRepository = webLocalDistrictSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteWebLocalProvinceRequest"/>, find existing <see cref="WebLocalProvince"/> base on id provided in <see cref="DeleteWebLocalProvinceRequest"/>,
        /// delete founded <see cref="WebLocalProvince"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteWebLocalProvinceRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWebLocalProvinceValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find webLocalProvince base on id provided from database, if webLocalProvince was not found, throw not found exception.
            // Need tracking to delete webLocalProvince.
            WebLocalProvince webLocalProvince = await webLocalProvinceSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            if (webLocalProvince is null)
            {
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_PROVINCE_ID_NOT_FOUND, WebLocalProvinceConst.MSG_LOCALPROVINCE_ID_NOT_FOUND);
            }

            var district = await webLocalDistrictSqlRepository.FindSingleAsync(x => x.ProvinceId == request.Id, false, cancellationToken);
            if (district is not null)
            {
                CustomException.ThrowConflictException(MsgCode.ERR_PROVINCE_HAS_DISTRICTS, WebLocalProvinceConst.MSG_CANNOT_DELETE_PROVINCE_WITH_DISTRICT);
            }

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked webLocalProvince as Deleted state
                webLocalProvinceSqlRepository.Remove(webLocalProvince);

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
