using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using MediatR;
using System.Collections.Generic;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    ///  Handler for <see cref="UpdateCompanyDepartmentHandler"/>/ 
    /// </summary>
    public class UpdateCompanyDepartmentHandler : IRequestHandler<UpdateCompanyDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyDepartment"/>>  /// </summary>
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly ICompanySqlRepository companySqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public UpdateCompanyDepartmentHandler(ICompanyDepartmentSqlRepository companyDepartmentSqlRepository, ICompanySqlRepository companySqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateCompanyDepartmentRequest"/>, find existing <see cref="Entities.ErpGeneralCompanyDepartment"/> base on id provided in <see cref="UpdateCompanyDepartmentRequest"/>,
        /// update founded <see cref="Entities.ErpGeneralCompanyDepartment"/> base on data provided in <see cref="UpdateCompanyDepartmentRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateCompanyDepartmentRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateCompanyDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company base on id provided from database, if Company was not found, throw not found exception.
            // Need tracking to update Company.
            Entities.ErpGeneralCompanyDepartment companyDepartment = await companyDepartmentSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (companyDepartment is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompanyDepartment), MsgCode.ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND);

            if (request.CompanyId is not null)
            {
                Entities.ErpGeneralCompany? company = await companySqlRepository.FindByIdAsync((int)request.CompanyId, true, cancellationToken);
                if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);
            }

            if (request.DepartmentId is not null)
            {
                var existingDepartment = await companyDepartmentSqlRepository.FindSingleAsync(x => x.CompanyId == request.CompanyId && x.DepartmentId == request.DepartmentId, false);
                if (existingDepartment is not null) CustomException.ThrowConflictException(MsgCode.ERR_DEPARTMENT_DUPLICATE_ID, ErpGeneralCompanyConst.MSG_DUPLICATE_DEPARTMENT_IN_DATABASE.FormatMsg(existingDepartment.DepartmentId));
            }

            // Update Company base on data provided in UpdateCompanyCommand request.
            // Keep Company original data if request fields is null
            request.MapTo(companyDepartment, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark Company as Updated state
                companyDepartmentSqlRepository.Update(companyDepartment!);

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