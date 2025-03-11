using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    public class GetDetailCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanyRepository;
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentRepository;
        private readonly Mock<IGeneralDepartmentSqlRepository> mockDepartmentRepository;
        private readonly Mock<ICompanyPositionSqlRepository> mockCompanyPositionRepository;
        private readonly Mock<IErpGeneralPositionSqlRepository> mockPositionRepository;
        private readonly GetDetailCompanyHandler handler;

        public GetDetailCompanyTest()
        {
            mockCompanyRepository = new Mock<ICompanySqlRepository>();
            mockCompanyDepartmentRepository = new Mock<ICompanyDepartmentSqlRepository>();
            mockDepartmentRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockCompanyPositionRepository = new Mock<ICompanyPositionSqlRepository>();
            mockPositionRepository = new Mock<IErpGeneralPositionSqlRepository>();

            handler = new GetDetailCompanyHandler(
                mockCompanyRepository.Object,
                mockCompanyDepartmentRepository.Object,
                mockDepartmentRepository.Object,
                mockPositionRepository.Object,
                mockCompanyPositionRepository.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnCompany_When_Found()
        {
            // Arrange
            var company = new Entities.ErpGeneralCompany
            {
                Id = 1,
                CompanyPid = 1,
                TaxCode = "1234567890",
                Name = "Company 1",
                Image = "company1.png",
                Tel = "123-456-7890",
                Email = "contact@company1.com",
                Website = "www.company1.com",
                Founder = "Founder 1",
                Ceo = "CEO 1",
                CeoImage = "ceo1.png",
                CeoEmail = "ceo1@company1.com",
                CeoTel = "098-765-4321",
                License = "ABC123456",
                CountryId = "AAA",
                WardId = 1
            };

            var companyDepartments = new List<Entities.ErpGeneralCompanyDepartment>
            {
                new() { CompanyId = 1, DepartmentId = 101 }
            };

            var departments = new List<Entities.GeneralDepartment>
            {
                new() { Id = 101, DeName = "Phòng Kinh doanh", DeCode = "KD01" }
            };

            var companyPositions = new List<Entities.ErpGeneralCompanyPosition>
            {
                new() { CompanyId = 1, PositionId = 201 }
            };

            var positions = new List<Entities.ErpGeneralPosition>
            {
                new() { Id = 201, Code = "POS01", Name = "Manager" }
            };

            mockCompanyRepository.Setup(repo => repo.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            mockCompanyDepartmentRepository.Setup(repo => repo.FindAll(null, false)).Returns(companyDepartments.AsQueryable());
            mockDepartmentRepository.Setup(repo => repo.FindAll(null, false)).Returns(departments.AsQueryable());
            mockCompanyPositionRepository.Setup(repo => repo.FindAll(null, false)).Returns(companyPositions.AsQueryable());
            mockPositionRepository.Setup(repo => repo.FindAll(null, false)).Returns(positions.AsQueryable());

            var request = new GetDetailCompanyRequest { Id = 1 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(company.Id, result.Data.Id);
            Assert.Equal("Company 1", result.Data.Name);
            Assert.Single(result.Data.Departments);
            Assert.Contains(result.Data.Departments, d => d.Id == 101 && d.DeName == "Phòng Kinh doanh");
            Assert.Single(result.Data.Positions);
            Assert.Contains(result.Data.Positions, p => p.Id == 201 && p.Name == "Manager");
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Company_NotFound()
        {
            // Arrange
            mockCompanyRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.ErpGeneralCompany)null);
            var request = new GetDetailCompanyRequest { Id = 99 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailCompanyRequest();

            var validator = new GetDetailCompanyValidator();

            // Act & Assert
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}