using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany
{
    public class GetAllCompanyHandler : IRequestHandler<GetAllCompanyRequest, Result<List<CompanyDTO>>>
    {
        private readonly ICompanySqlRepository companySqlRepository;
        private readonly ICompanyDepartmentSqlRepository companyDepartmentSqlRepository;
        private readonly IGeneralDepartmentSqlRepository departmentSqlRepository;

        public GetAllCompanyHandler(
            ICompanySqlRepository companySqlRepository,
            ICompanyDepartmentSqlRepository companyDepartmentSqlRepository,
            IGeneralDepartmentSqlRepository departmentSqlRepository)
        {
            this.companySqlRepository = companySqlRepository;
            this.companyDepartmentSqlRepository = companyDepartmentSqlRepository;
            this.departmentSqlRepository = departmentSqlRepository;
        }

        public async Task<Result<List<CompanyDTO>>> Handle(GetAllCompanyRequest request, CancellationToken cancellationToken)
        {
            var companies = companySqlRepository.FindAll().ToList();
            var companyDepartments = companyDepartmentSqlRepository.FindAll().ToList();
            var departments = departmentSqlRepository.FindAll().ToList();

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
                    }).ToList()
            }).ToList();

            return Result<List<CompanyDTO>>.Ok(companyDtos);
        }
    }
}