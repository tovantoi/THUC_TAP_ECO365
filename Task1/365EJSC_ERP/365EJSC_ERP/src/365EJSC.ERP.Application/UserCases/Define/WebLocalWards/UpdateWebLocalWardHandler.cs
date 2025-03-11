using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Constants;
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

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    /// <summary>
    /// Handler for <see cref="UpdateWebLocalWardRequest"/>
    /// </summary>
    public class UpdateWebLocalWardHandler : IRequestHandler<UpdateWebLocalWardRequest, Result<object>>
    {
        /// <summary>
        /// Repository handling data access of <see cref="WebLocalWard"/>
        /// </summary>
        private readonly IWebLocalWardSqlRepository wardSqlRepository;

        /// <summary>
        /// Repository handling data access of <see cref="WebLocalDistrict"/>
        /// </summary>
        private readonly IWebLocalDistrictSqlRepository dictrictSqlRepository;

        /// <summary>
        /// Unit of work to handle transactions
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public UpdateWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, ISqlUnitOfWork sqlUnitOfWork, IWebLocalDistrictSqlRepository dictrictSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.dictrictSqlRepository = dictrictSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="UpdateWebLocalWardRequest"/>, update an existing <see cref="WebLocalWard"/>
        /// based on data in <see cref="UpdateWebLocalWardRequest"/> and save changes to the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(UpdateWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find the existing ward by ID
            WebLocalWard ward = await wardSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Map updated data from request to the ward entity
            request.MapTo(ward, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Check if the DistrictId exists before updating
                if (request.DistrictId.HasValue && !await dictrictSqlRepository.IsExistAsync(x => x.Id == ward.DistrictId))
                {
                    var errorMessage = MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(WebLocalWardConst.FIELD_DISTRICT_ID);
                    CustomException.ThrowNotFoundException(typeof(WebLocalWard), MsgCode.ERR_NF_FIND_KEY, errorMessage);
                }

                // Update the ward entity
                wardSqlRepository.Update(ward);

                // Save changes to the database
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
