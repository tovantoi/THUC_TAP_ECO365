using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyDepartment
{
    public class GetAllCompanyDepartmentTest
    {
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentRepository;
        private readonly GetAllCompanyDepartmentHandler handler;

        public GetAllCompanyDepartmentTest()
        {
            mockCompanyDepartmentRepository = new Mock<ICompanyDepartmentSqlRepository>();
            handler = new GetAllCompanyDepartmentHandler(mockCompanyDepartmentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllCompanyDepartments()
        {
            // Arrange
            var companyDepartments = new List<Entities.ErpGeneralCompanyDepartment>
            {
                new() { Id = 1, CompanyId = 1, DepartmentId = 1 },
                new() { Id = 2, CompanyId = 2, DepartmentId = 2 }
            };

            mockCompanyDepartmentRepository.Setup(repository => repository.FindAll(null, false)).Returns(companyDepartments.AsQueryable());

            var query = new GetAllCompanyDepartmentRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.CompanyId == 1 && s.DepartmentId == 1);
            Assert.Contains(result.Data, s => s.Id == 2 && s.CompanyId == 2 && s.DepartmentId == 2);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoCompanyDepartments()
        {
            // Arrange
            mockCompanyDepartmentRepository.Setup(repository => repository.FindAll(null, false)).Returns(new List<Entities.ErpGeneralCompanyDepartment>().AsQueryable());

            var query = new GetAllCompanyDepartmentRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}