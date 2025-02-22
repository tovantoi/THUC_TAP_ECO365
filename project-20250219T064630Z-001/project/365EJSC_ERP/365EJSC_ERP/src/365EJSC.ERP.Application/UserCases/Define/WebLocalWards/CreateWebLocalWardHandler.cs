using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Constants;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateWebLocalWardRequest"/>/ </summary>
    public class CreateWebLocalWardHandler : IRequestHandler<CreateWebLocalWardRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocalWard"/>>  /// </summary>
        private readonly IWebLocalWardSqlRepository wardSqlRepository;
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocalWard"/>>  /// </summary>
        private readonly IWebLocalDistrictSqlRepository dictrictSqlRepository;
        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, ISqlUnitOfWork sqlUnitOfWork, IWebLocalDistrictSqlRepository dictrictSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.dictrictSqlRepository = dictrictSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="CreateWebLocalWardRequest"/>, create new <see cref="WebLocalWard"/> base on data <see cref="CreateWebLocalWardRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new ward from request
            WebLocalWard? ward = request.MapTo<WebLocalWard>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var dictrictIdExists = await dictrictSqlRepository.IsExistAsync(x => x.Id == request.DistrictId);
                if (!dictrictIdExists)
                {
                    var errorMessage = MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(WebLocalWardConst.FIELD_DISTRICT_ID);
                    CustomException.ThrowNotFoundException(typeof(WebLocalWard), MsgCode.ERR_DISTRICT_ID_NOT_FOUND, errorMessage);
                }

                // Marked sample as Created state
                wardSqlRepository.Add(ward);

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
