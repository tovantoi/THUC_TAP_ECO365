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
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateCompanyDepartmentRequest"/>/ </summary>
    public class CreateCompanyDepartmentHandler : IRequestHandler<CreateCompanyDepartmentRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyDepartment"/>>  /// </summary>
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly IGeneralDepartmentSqlRepository departmentSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyDepartmentHandler"/>, inject needed dependency
        /// </summary>
        public CreateCompanyDepartmentHandler(ICompanyDepartmentSqlRepository companyDepartmentSqlRepository, ICompanySqlRepository companySqlRepository, ISqlUnitOfWork sqlUnitOfWork, IGeneralDepartmentSqlRepository departmentSqlRepository)
        {
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.departmentSqlRepository = departmentSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="CreateCompanyDepartmentRequest"/>, create new <see cref="Entities.ErpGeneralCompany"/> base on data <see cref="CreateCompanyDepartmentRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateCompanyDepartmentRequest request, CancellationToken cancellationToken)
        {
            CreateCompanyDepartmentValidator validator = new();
            validator.ValidateAndThrow(request);

            Entities.ErpGeneralCompany? company = await companySqlRepository.FindByIdAsync(request.CompanyId, true, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);

            List<Entities.GeneralDepartment>? departments = await departmentSqlRepository.FindByIds(request.DepartmentIds.ToList(), false, cancellationToken);
            List<int>? invalidDepartmentIds = request.DepartmentIds.Except(departments.Select(d => d.Id)).ToList();
            if (invalidDepartmentIds.Any()) CustomException.ThrowNotFoundException(typeof(Entities.GeneralDepartment), MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND, ErpGeneralCompanyConst.MSG_DEPARTMENT_ID_NOT_FOUND.FormatMsg(string.Join(", ", invalidDepartmentIds)));

            List<int>? duplicateDepartmetnIds = request.DepartmentIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateDepartmetnIds.Any()) CustomException.ThrowConflictException(MsgCode.ERR_DEPARTMENT_DUPLICATE_ID, ErpGeneralCompanyConst.MSG_DUPLICATE_ID.FormatMsg(string.Join(", ", duplicateDepartmetnIds)));

            List<int> existingDepartmentIds = companyDepartmentSqlRepository.FindAll(x => x.CompanyId == request.CompanyId).Select(x => x.DepartmentId).ToList();
            List<int> duplicateCompaDepart = request.DepartmentIds.Intersect(existingDepartmentIds).ToList();
            if (duplicateCompaDepart.Any()) CustomException.ThrowConflictException(MsgCode.ERR_DEPARTMENT_DUPLICATE_ID, ErpGeneralCompanyConst.MSG_DUPLICATE_DEPARTMENT_IN_DATABASE.FormatMsg(string.Join(", ", duplicateCompaDepart)));
            
            // Create new CompanyDepartment from request
            List<Entities.ErpGeneralCompanyDepartment>? companyDepartment = request.DepartmentIds.Select(departments => new Entities.ErpGeneralCompanyDepartment
            {
                CompanyId = request.CompanyId,
                DepartmentId = departments
            }).ToList();
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Company as Created state
                companyDepartmentSqlRepository.AddRange(companyDepartment);

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