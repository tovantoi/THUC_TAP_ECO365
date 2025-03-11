using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    /// <summary>
    /// Handler for <see cref="DeleteCompanyRequest"/>
    /// </summary>
    public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Domain.Entities.Define.ErpGeneralCompany"/>>  /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="DeleteCompanyHandler"/>, inject needed dependency
        /// </summary>
        public DeleteCompanyHandler(ICompanySqlRepository companySqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteCompanyValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Domain.Entities.Define.ErpGeneralCompany company = await companySqlRepository.FindSingleAsync(x => x.Id == request.Id && x.IsActived == true, true, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Domain.Entities.Define.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Change actived to unactived
                company.IsActived = false;

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