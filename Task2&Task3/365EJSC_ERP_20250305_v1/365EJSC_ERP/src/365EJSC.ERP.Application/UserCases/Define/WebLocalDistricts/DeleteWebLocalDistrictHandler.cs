using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts;
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

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts
{
    /// <summary>
    /// Handler for <see cref="DeleteWebLocalDistrictRequest"/>
    /// </summary>
    public class DeleteWebLocalDistrictHandler : IRequestHandler<DeleteWebLocalDistrictRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="District"/>
        /// </summary>
        private readonly IWebLocalDistrictSqlRepository districtSqlRepository;

        /// <summary>
        /// Repository handle data access of <see cref="WebLocalWard"/>
        /// </summary>
        private readonly IWebLocalWardSqlRepository wardSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteDistrictHandler"/>, inject needed dependency
        /// </summary>
        public DeleteWebLocalDistrictHandler(IWebLocalDistrictSqlRepository districtSqlRepository,IWebLocalWardSqlRepository wardSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.districtSqlRepository = districtSqlRepository;
            this.wardSqlRepository = wardSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="DeleteWebLocalDistrictRequest"/>, find existing <see cref="District"/> based on id provided in <see cref="DeleteWebLocalDistrictRequest"/>,
        /// delete the found <see cref="District"/> and save to the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteWebLocalDistrictRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWebLocalDistrictValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find district based on id provided from database, if district was not found, throw not found exception.
            // Need tracking to delete district.
            WebLocalDistrict district = await districtSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Check if any ward is using this district
            bool hasWards = await wardSqlRepository.IsExistAsync(w => w.DistrictId == district.Id, cancellationToken);
            if (hasWards)
            {
                CustomException.ThrowNotFoundException(
                 typeof(WebLocalDistrict),
                 MsgCode.ERR_DISTRICT_HAS_WARDS,
                 WebLocalDistrictConst.MSG_CANNOT_DELETE_DISTRICT_WITH_WARDS
             );
            }

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked district as Deleted state
                districtSqlRepository.Remove(district);

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
