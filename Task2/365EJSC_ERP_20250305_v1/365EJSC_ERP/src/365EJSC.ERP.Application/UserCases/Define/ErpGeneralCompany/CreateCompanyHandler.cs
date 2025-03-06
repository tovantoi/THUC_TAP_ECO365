using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
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

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateCompanyRequest"/>/ </summary>
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Domain.Entities.Define.ErpGeneralCompany"/>>  /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;
        private readonly IWebLocalWardSqlRepository webLocalWardSqlRepository;
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyHandler"/>, inject needed dependency
        /// </summary>
        public CreateCompanyHandler(ICompanySqlRepository companySqlRepository, 
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
        /// Handle <see cref="CreateCompanyRequest"/>, create new <see cref="Domain.Entities.Define.ErpGeneralCompany"/> base on data <see cref="CreateCompanyRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateCompanyValidator validator = new();
            validator.ValidateAndThrow(request);

            Entities.WebLocals? local = await webLocalSqlRepository.FindByIdAsync(request.CountryId, true, cancellationToken);
            if (local is null) CustomException.ThrowNotFoundException(null, MsgCode.ERR_KEY_LOCAL_NOT_FOUND, ErpGeneralCompanyConst.MSG_COUNTRY_ID_NOT_FOUND);

            Entities.WebLocalWard? ward = await webLocalWardSqlRepository.FindByIdAsync((int)request.WardId, true, cancellationToken);
            if (ward is null) CustomException.ThrowNotFoundException(typeof(Entities.WebLocalWard), MsgCode.ERR_WARD_ID_NOT_FOUND);

            // Create new Company from request
            Entities.ErpGeneralCompany? company = request.MapTo<Entities.ErpGeneralCompany>();
            company.IsActived = request.IsActived ?? true;
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Company as Created state
                companySqlRepository.Add(company);

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