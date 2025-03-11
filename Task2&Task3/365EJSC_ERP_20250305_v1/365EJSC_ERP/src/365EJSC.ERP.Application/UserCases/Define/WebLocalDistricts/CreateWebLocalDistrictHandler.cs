using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
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
    /// Handler for <see cref="CreateWebLocalDistrictRequest"/>
    /// </summary>
    public class CreateWebLocalDistrictHandler : IRequestHandler<CreateWebLocalDistrictRequest, Result<object>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="District"/>
        /// </summary>
        private readonly IWebLocalDistrictSqlRepository districtSqlRepository;

        /// <summary>
        /// Repository for checking existence of Province
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository provinceSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateWebLocalDistrictHandler"/>, inject needed dependency
        /// </summary>
        public CreateWebLocalDistrictHandler(IWebLocalDistrictSqlRepository districtSqlRepository,
                                             IWebLocalProvinceSqlRepository provinceSqlRepository, 
                                             ISqlUnitOfWork sqlUnitOfWork)
        {
            this.districtSqlRepository = districtSqlRepository;
            this.provinceSqlRepository = provinceSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="CreateWebLocalDistrictRequest"/>, create new <see cref="District"/> based on data <see cref="CreateWebLocalDistrictRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateWebLocalDistrictRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateWebLocalDistrictValidator validator = new();
            validator.ValidateAndThrow(request);

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


            // Create new district from request
            WebLocalDistrict? district = request.MapTo<WebLocalDistrict>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked district as Created state
                districtSqlRepository.Add(district);

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
