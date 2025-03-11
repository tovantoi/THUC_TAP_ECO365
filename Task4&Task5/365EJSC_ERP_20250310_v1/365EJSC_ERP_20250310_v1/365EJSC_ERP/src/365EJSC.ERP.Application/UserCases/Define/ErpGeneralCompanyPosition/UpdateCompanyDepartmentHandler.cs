using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    ///  Handler for <see cref="UpdateCompanyPositionHandler"/>/ 
    /// </summary>
    public class UpdateCompanyPositionHandler : IRequestHandler<UpdateCompanyPositionRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyPosition"/>>  /// </summary>
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;
        private readonly ICompanySqlRepository companySqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyPositionHandler"/>, inject needed dependency
        /// </summary>
        public UpdateCompanyPositionHandler(ICompanyPositionSqlRepository companyPositionSqlRepository, ICompanySqlRepository companySqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.companyPositionSqlRepository = companyPositionSqlRepository;
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateCompanyPositionRequest"/>, find existing <see cref="Entities.ErpGeneralCompanyPosition"/> base on id provided in <see cref="UpdateCompanyPositionRequest"/>,
        /// update founded <see cref="Entities.ErpGeneralCompanyPosition"/> base on data provided in <see cref="UpdateCompanyPositionRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateCompanyPositionRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateCompanyPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Entities.ErpGeneralCompanyPosition companyPosition = await companyPositionSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (companyPosition is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyPosition), MsgCode.ERR_COMPANY_POSITION_ID_NOT_FOUND);

            if (request.CompanyId is not null)
            {
                Entities.ErpGeneralCompany? company = await companySqlRepository.FindByIdAsync((int)request.CompanyId, true, cancellationToken);
                if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);
            }

            if (request.PositionId is not null)
            {
                var existingPosition = await companyPositionSqlRepository.FindSingleAsync(x => x.CompanyId == request.CompanyId && x.PositionId == request.PositionId, false);
                if (existingPosition is not null) CustomException.ThrowConflictException(MsgCode.ERR_POSITION_DUPLICATE_ID, ErpGeneralCompanyPositionConst.MSG_DUPLICATE_POSITION_IN_DATABASE.FormatMsg(existingPosition.PositionId));
            }

            // Update Company base on data provided in UpdateCompanyCommand request.
            // Keep Company original data if request fields is null
            request.MapTo(companyPosition, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Company as Updated state
                companyPositionSqlRepository.Update(companyPosition!);

                // Save Company to database
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