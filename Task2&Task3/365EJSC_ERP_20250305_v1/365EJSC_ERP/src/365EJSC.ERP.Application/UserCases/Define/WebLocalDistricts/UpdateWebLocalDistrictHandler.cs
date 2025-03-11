using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts
{
    /// <summary>
    /// Handler for <see cref="UpdateWebLocalDistrictRequest"/>
    /// </summary>
    public class UpdateWebLocalDistrictHandler : IRequestHandler<UpdateWebLocalDistrictRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="District"/>
        /// </summary>
        private readonly IWebLocalDistrictSqlRepository districtRepository;

        /// <summary>
        /// Repository for checking existence of Province
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository provinceSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalDistrictHandler"/>, inject needed dependency
        /// </summary>
        public UpdateWebLocalDistrictHandler(IWebLocalDistrictSqlRepository districtRepository,
                                             IWebLocalProvinceSqlRepository provinceSqlRepository, 
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.districtRepository = districtRepository;
            this.provinceSqlRepository = provinceSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateWebLocalDistrictRequest"/>, find existing <see cref="District"/> based on id provided in <see cref="UpdateWebLocalDistrictRequest"/>,
        /// update the found <see cref="District"/> based on data provided in <see cref="UpdateWebLocalDistrictRequest"/> and save to the database.
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(UpdateWebLocalDistrictRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateWebLocalDistrictValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find district based on id provided from the database, if district is not found, throw not found exception.
            // Need tracking to update the district.
            WebLocalDistrict district = await districtRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Check if province exists
            if (request.ProvinceId != null)
            {
                var provinceExists = await provinceSqlRepository.IsExistAsync(x => x.Id == request.ProvinceId);
                if (!provinceExists)
                {
                    CustomException.ThrowNotFoundException(
                        typeof(WebLocalProvince),
                        MsgCode.ERR_NOT_FOUND,
                        WebLocalProvinceConst.MSG_LOCALPROVINCE_ID_NOT_FOUND
                    );
                }
            }

            // Update district based on data provided in UpdateWebLocalDistrictRequest request.
            // Keep district original data if request fields are null
            request.MapTo(district, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark district as Updated state
                districtRepository.Update(district!);

                // Save district to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happens, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }

}
