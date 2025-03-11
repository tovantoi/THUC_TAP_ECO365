using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    public class GetAllCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanyRepository;
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentRepository;
        private readonly Mock<IGeneralDepartmentSqlRepository> mockDepartmentRepository;
        private readonly GetAllCompanyHandler handler;

        public GetAllCompanyTest()
        {
            mockCompanyRepository = new Mock<ICompanySqlRepository>();
            mockCompanyDepartmentRepository = new Mock<ICompanyDepartmentSqlRepository>();
            mockDepartmentRepository = new Mock<IGeneralDepartmentSqlRepository>();

            handler = new GetAllCompanyHandler(
                mockCompanyRepository.Object,
                mockCompanyDepartmentRepository.Object,
                mockDepartmentRepository.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnAllCompanies_WithDepartments()
        {
            // Arrange
            var companies = new List<Entities.ErpGeneralCompany>
            {
                new() { Id = 1, CompanyPid = 1, Name = "Company 1", TaxCode = "1234567890", Ceo = "CEO 1" },
                new() { Id = 2, CompanyPid = 2, Name = "Company 2", TaxCode = "0987654321", Ceo = "CEO 2" }
            };

            var companyDepartments = new List<Entities.ErpGeneralCompanyDepartment>
            {
                new() { CompanyId = 1, DepartmentId = 101 },
                new() { CompanyId = 1, DepartmentId = 102 },
                new() { CompanyId = 2, DepartmentId = 103 }
            };

            var departments = new List<Entities.GeneralDepartment>
            {
                new() { Id = 101, DeName = "Phòng Kinh doanh", DeCode = "KD01" },
                new() { Id = 102, DeName = "Phòng Kế toán", DeCode = "KT01" },
                new() { Id = 103, DeName = "Phòng Nhân sự", DeCode = "NS01" }
            };

            mockCompanyRepository.Setup(repo => repo.FindAll(null, false)).Returns(companies.AsQueryable());
            mockCompanyDepartmentRepository.Setup(repo => repo.FindAll(null, false)).Returns(companyDepartments.AsQueryable());
            mockDepartmentRepository.Setup(repo => repo.FindAll(null, false)).Returns(departments.AsQueryable());

            var query = new GetAllCompanyRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);

            var company1 = result.Data.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(company1);
            Assert.Equal("Company 1", company1.Name);
            Assert.Equal(2, company1.Departments.Count);
            Assert.Contains(company1.Departments, d => d.Id == 101 && d.DeName == "Phòng Kinh doanh");
            Assert.Contains(company1.Departments, d => d.Id == 102 && d.DeName == "Phòng Kế toán");

            var company2 = result.Data.FirstOrDefault(c => c.Id == 2);
            Assert.NotNull(company2);
            Assert.Equal("Company 2", company2.Name);
            Assert.Single(company2.Departments);
            Assert.Contains(company2.Departments, d => d.Id == 103 && d.DeName == "Phòng Nhân sự");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoCompaniesExist()
        {
            // Arrange
            mockCompanyRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Entities.ErpGeneralCompany>().AsQueryable());
            mockCompanyDepartmentRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Entities.ErpGeneralCompanyDepartment>().AsQueryable());
            mockDepartmentRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Entities.GeneralDepartment>().AsQueryable());

            var query = new GetAllCompanyRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}