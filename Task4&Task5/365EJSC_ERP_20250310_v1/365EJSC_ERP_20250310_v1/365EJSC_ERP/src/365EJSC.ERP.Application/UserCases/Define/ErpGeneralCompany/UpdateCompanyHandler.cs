using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    /// <summary>
    ///  Handler for <see cref="UpdateCompanyHandler"/>/ 
    /// </summary>
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompany"/>>  /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;
        private readonly IWebLocalWardSqlRepository webLocalWardSqlRepository;
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Constructor of <see cref="UpdateCompanyHandler"/>, inject needed dependency
        /// </summary>
        public UpdateCompanyHandler(ICompanySqlRepository companySqlRepository, 
                                    ISqlUnitOfWork sqlUnitOfWork, 
                                    IWebLocalWardSqlRepository webLocalWardSqlRepository, 
                                    IWebLocalSqlRepository webLocalSqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.webLocalWardSqlRepository = webLocalWardSqlRepository;
            this.webLocalSqlRepository = webLocalSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="UpdateCompanyRequest"/>, find existing <see cref="Entities.ErpGeneralCompany"/> base on id provided in <see cref="UpdateCompanyRequest"/>,
        /// update founded <see cref="Entities.ErpGeneralCompany"/> base on data provided in <see cref="UpdateCompanyRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateCompanyValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Entities.ErpGeneralCompany company = await companySqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);

            if (request.CountryId is not null || request.WardId is not null)
            {
                Entities.WebLocals? local = await webLocalSqlRepository.FindByIdAsync(request.CountryId, true, cancellationToken);
                if (local is null) CustomException.ThrowNotFoundException(typeof(Entities.WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND);

                Entities.WebLocalWard? ward = await webLocalWardSqlRepository.FindByIdAsync((int)request.WardId, true, cancellationToken);
                if (ward is null) CustomException.ThrowNotFoundException(typeof(Entities.WebLocalWard), MsgCode.ERR_WARD_ID_NOT_FOUND);
            }
            // Update Company base on data provided in UpdateCompanyCommand request.
            // Keep Company original data if request fields is null
            request.MapTo(company, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Company as Updated state
                companySqlRepository.Update(company!);

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