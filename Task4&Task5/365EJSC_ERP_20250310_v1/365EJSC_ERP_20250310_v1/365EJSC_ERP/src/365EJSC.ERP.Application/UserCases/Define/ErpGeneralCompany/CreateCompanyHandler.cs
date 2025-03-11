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
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompany"/>>  /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly IWebLocalWardSqlRepository webLocalWardSqlRepository;
        private readonly IWebLocalSqlRepository webLocalSqlRepository;
        private readonly IGeneralDepartmentSqlRepository departmentSqlRepository;
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly IErpGeneralPositionSqlRepository positionSqlRepository;
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyHandler"/>, inject needed dependency
        /// </summary>
        public CreateCompanyHandler(ICompanySqlRepository companySqlRepository,
                                    ISqlUnitOfWork sqlUnitOfWork,
                                    IWebLocalWardSqlRepository webLocalWardSqlRepository,
                                    IWebLocalSqlRepository webLocalSqlRepository,
                                    IGeneralDepartmentSqlRepository departmentSqlRepository,
                                    ICompanyDepartmentSqlRepository companyDepartmentSqlRepository,
                                    IErpGeneralPositionSqlRepository positionSqlRepository,
                                    ICompanyPositionSqlRepository companyPositionSqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.webLocalWardSqlRepository = webLocalWardSqlRepository;
            this.webLocalSqlRepository = webLocalSqlRepository;
            this.departmentSqlRepository = departmentSqlRepository;
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.positionSqlRepository = positionSqlRepository;
            this.companyPositionSqlRepository = companyPositionSqlRepository;
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
                if (request.DepartmentIds is not null)
                {
                    List<Entities.GeneralDepartment>? departments = await departmentSqlRepository.FindByIds(request.DepartmentIds.ToList(), false, cancellationToken);
                    List<int>? invalidDepartmentIds = request.DepartmentIds.Except(departments.Select(d => d.Id)).ToList();
                    if (invalidDepartmentIds.Any()) CustomException.ThrowNotFoundException(typeof(Entities.GeneralDepartment), MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND, ErpGeneralCompanyConst.MSG_DEPARTMENT_ID_NOT_FOUND.FormatMsg(string.Join(", ", invalidDepartmentIds)));

                    List<int>? duplicateDepartmetnIds = request.DepartmentIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (duplicateDepartmetnIds.Any()) CustomException.ThrowConflictException(MsgCode.ERR_DEPARTMENT_DUPLICATE_ID, ErpGeneralCompanyConst.MSG_DUPLICATE_ID.FormatMsg(string.Join(", ", duplicateDepartmetnIds)));

                    company.CompanyDepartments = request.DepartmentIds?.Distinct().Select(departmentId => new Entities.ErpGeneralCompanyDepartment
                        {
                            CompanyId = company.Id,
                            DepartmentId = departmentId
                        }).ToList();
                }

                if (request.PositionIds is not null)
                {
                    List<Entities.ErpGeneralPosition>? Positions = await positionSqlRepository.FindByIds(request.PositionIds.ToList(), false, cancellationToken);
                    List<int>? invalidPositionIds = request.PositionIds.Except(Positions.Select(d => d.Id)).ToList();
                    if (invalidPositionIds.Any()) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralPosition), MsgCode.ERR_POSITION_ID_NOT_FOUND, ErpGeneralCompanyPositionConst.MSG_POSITION_ID_NOT_FOUND.FormatMsg(string.Join(", ", invalidPositionIds)));

                    List<int>? duplicateDepartmetnIds = request.PositionIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (duplicateDepartmetnIds.Any()) CustomException.ThrowConflictException(MsgCode.ERR_POSITION_DUPLICATE_ID, ErpGeneralCompanyPositionConst.MSG_DUPLICATE_ID.FormatMsg(string.Join(", ", duplicateDepartmetnIds)));

                    company.CompanyPositions = request.PositionIds?.Distinct().Select(positionId => new Entities.ErpGeneralCompanyPosition
                    {
                        CompanyId = company.Id,
                        PositionId = positionId
                    }).ToList();
                }

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