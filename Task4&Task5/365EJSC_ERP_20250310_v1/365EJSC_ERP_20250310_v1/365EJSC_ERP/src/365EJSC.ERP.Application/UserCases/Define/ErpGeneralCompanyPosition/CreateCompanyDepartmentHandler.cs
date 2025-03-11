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
    /// Hand/// Handler for <see cref="CreateCompanyPositionRequest"/>/ </summary>
    public class CreateCompanyPositionHandler : IRequestHandler<CreateCompanyPositionRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="Entities.ErpGeneralCompanyPosition"/>>  /// </summary>
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly IErpGeneralPositionSqlRepository positionSqlRepository;

        /// <summary>
        /// Unit of work to handle transaction
        /// </summary>
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        /// <summary>
        /// Constructor of <see cref="CreateCompanyPositionHandler"/>, inject needed dependency
        /// </summary>
        public CreateCompanyPositionHandler(ICompanyPositionSqlRepository companyPositionSqlRepository, ICompanySqlRepository companySqlRepository, ISqlUnitOfWork sqlUnitOfWork, IErpGeneralPositionSqlRepository positionSqlRepository)
        {
            this.companyPositionSqlRepository = companyPositionSqlRepository;
            this.companySqlRepository = companySqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.positionSqlRepository = positionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="CreateCompanyPositionRequest"/>, create new <see cref="Entities.ErpGeneralCompany"/> base on data <see cref="CreateCompanyPositionRequest"/>
        /// and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<object>> Handle(CreateCompanyPositionRequest request, CancellationToken cancellationToken)
        {
            CreateCompanyPositionValidator validator = new();
            validator.ValidateAndThrow(request);

            Entities.ErpGeneralCompany? company = await companySqlRepository.FindByIdAsync(request.CompanyId, true, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);

            List<Entities.ErpGeneralPosition>? Positions = await positionSqlRepository.FindByIds(request.PositionIds.ToList(), false, cancellationToken);
            List<int>? invalidPositionIds = request.PositionIds.Except(Positions.Select(d => d.Id)).ToList();
            if (invalidPositionIds.Any()) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralPosition), MsgCode.ERR_POSITION_ID_NOT_FOUND, ErpGeneralCompanyPositionConst.MSG_POSITION_ID_NOT_FOUND.FormatMsg(string.Join(", ", invalidPositionIds)));

            List<int>? duplicateDepartmetnIds = request.PositionIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateDepartmetnIds.Any()) CustomException.ThrowConflictException(MsgCode.ERR_POSITION_DUPLICATE_ID, ErpGeneralCompanyPositionConst.MSG_DUPLICATE_ID.FormatMsg(string.Join(", ", duplicateDepartmetnIds)));

            List<int> existingPositionIds = companyPositionSqlRepository.FindAll(x => x.CompanyId == request.CompanyId).Select(x => x.PositionId.Value).ToList();
            List<int> duplicateCompaDepart = request.PositionIds.Intersect(existingPositionIds).ToList();
            if (duplicateCompaDepart.Any()) CustomException.ThrowConflictException(MsgCode.ERR_POSITION_DUPLICATE_ID, ErpGeneralCompanyPositionConst.MSG_DUPLICATE_POSITION_IN_DATABASE.FormatMsg(string.Join(", ", duplicateCompaDepart)));
            
            // Create new CompanyPosition from request
            List<Entities.ErpGeneralCompanyPosition>? companyPosition = request.PositionIds.Select(Positions => new Entities.ErpGeneralCompanyPosition
            {
                CompanyId = request.CompanyId,
                PositionId = Positions
            }).ToList();
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Company as Created state
                companyPositionSqlRepository.AddRange(companyPosition);

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