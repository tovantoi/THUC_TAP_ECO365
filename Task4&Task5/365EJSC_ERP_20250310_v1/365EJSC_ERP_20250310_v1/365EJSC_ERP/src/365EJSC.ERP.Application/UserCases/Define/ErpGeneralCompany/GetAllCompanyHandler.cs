using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    public class GetAllCompanyHandler : IRequestHandler<GetAllCompanyRequest, Result<List<CompanyDTO>>>
    {
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly IGeneralDepartmentSqlRepository departmentSqlRepository;
        private readonly IErpGeneralPositionSqlRepository positionSqlRepository;
        private readonly ICompanyPositionSqlRepository companyPositionSqlRepository;

        public GetAllCompanyHandler(ICompanySqlRepository companySqlRepository,
                                    ICompanyDepartmentSqlRepository companyDepartmentSqlRepository,
                                    IGeneralDepartmentSqlRepository departmentSqlRepository,
                                    IErpGeneralPositionSqlRepository positionSqlRepository,
                                    ICompanyPositionSqlRepository companyPositionSqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.departmentSqlRepository = departmentSqlRepository;
            this.positionSqlRepository = positionSqlRepository;
            this.companyPositionSqlRepository = companyPositionSqlRepository;
        }

        public async Task<Result<List<CompanyDTO>>> Handle(GetAllCompanyRequest request, CancellationToken cancellationToken)
        {
            List<Entities.ErpGeneralCompany>? companies = companySqlRepository.FindAll().ToList();
            List<Entities.ErpGeneralCompanyDepartment>? companyDepartments = companyDepartmentSqlRepository.FindAll().ToList();
            List<Entities.ErpGeneralCompanyPosition>? companyPositions = companyPositionSqlRepository.FindAll().ToList();
            List<Entities.GeneralDepartment>? departments = departmentSqlRepository.FindAll().ToList();
            List<Entities.ErpGeneralPosition>? positions = positionSqlRepository.FindAll().ToList();

            var companyDtos = companies.Select(company => new CompanyDTO
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
            }).ToList();

            return Result<List<CompanyDTO>>.Ok(companyDtos);
        }
    }
}