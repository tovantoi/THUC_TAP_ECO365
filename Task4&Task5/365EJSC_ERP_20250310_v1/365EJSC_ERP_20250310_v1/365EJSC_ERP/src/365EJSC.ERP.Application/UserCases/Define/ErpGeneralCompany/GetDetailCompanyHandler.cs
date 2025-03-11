using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    /// <summary>
    /// Handler for <see cref="GetDetailCompanyQuery"/>
    /// </summary>
    public class GetDetailCompanyHandler : IRequestHandler<GetDetailCompanyRequest, Result<CompanyDTO>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="Entities.ErpGeneralCompany"/>>
        /// </summary>
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly IGeneralDepartmentSqlRepository departmentSqlRepository;
        private readonly IErpGeneralPositionSqlRepository positionSqlRepository;
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        // <summary>
        /// Constructor of <see cref="GetAllCompanyHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailCompanyHandler(ICompanySqlRepository companySqlRepository, ICompanyDepartmentSqlRepository companyDepartmentSqlRepository, IGeneralDepartmentSqlRepository departmentSqlRepository, IErpGeneralPositionSqlRepository positionSqlRepository, ICompanyPositionSqlRepository companyPositionSqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.departmentSqlRepository = departmentSqlRepository;
            this.positionSqlRepository = positionSqlRepository;
            this.companyPositionSqlRepository = companyPositionSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailCompanyQuery"/>, get <see cref="Entities.ErpGeneralCompany"/> from database with id provided in <see cref="GetDetailCompanyQuery"/>.
        /// Throw not found exception when <see cref="Entities.ErpGeneralCompany"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Entities.ErpGeneralCompany"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<CompanyDTO>> Handle(GetDetailCompanyRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailCompanyValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find Company by id provided. If Company not found will throw NotFoundException
            Entities.ErpGeneralCompany? company = await companySqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken);
            if (company is null) CustomException.ThrowNotFoundException(typeof(Entities.ErpGeneralCompany), MsgCode.ERR_COMPANY_ID_NOT_FOUND);

            List<Entities.ErpGeneralCompanyDepartment>? companyDepartments = companyDepartmentSqlRepository.FindAll().ToList();
            List<Entities.ErpGeneralCompanyPosition>? companyPositions = companyPositionSqlRepository.FindAll().ToList();
            List<Entities.GeneralDepartment>? departments = departmentSqlRepository.FindAll().ToList();
            List<Entities.ErpGeneralPosition>? positions = positionSqlRepository.FindAll().ToList();

            var companyDto = new CompanyDTO
            {
                Id = company.Id,
                CompanyPid = company.CompanyPid,
                TaxCode = company.TaxCode,
                Name = company.Name,
                Image = company.Image,
                Tel = company.Tel,
                Email = company.Email,
                Website = company.Website,
                Founder = company.Founder,
                Ceo = company.Ceo,
                CeoImage = company.CeoImage,
                CeoEmail = company.CeoEmail,
                CeoTel = company.CeoTel,
                License = company.License,
                CountryId = company.CountryId,
                WardId = company.WardId,
                Departments = companyDepartments.Where(cd => cd.CompanyId == company.Id)
                    .Join(departments, cd => cd.DepartmentId, d => d.Id, (cd, d) => new DepartmentDTO
                    {
                        Id = d.Id,
                        DeName = d.DeName,
                        DeCode = d.DeCode
                    }).ToList(),
                            Positions = companyPositions.Where(cp => cp.CompanyId == company.Id)
                    .Join(positions, cp => cp.PositionId, p => p.Id, (cp, p) => new PositionDTO
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                    }).ToList(),
            };

            return Result<CompanyDTO>.Ok(companyDto); ;
        }
    }
}