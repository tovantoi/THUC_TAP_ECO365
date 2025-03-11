using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.GeneralGeneralDepartments
{
    public class GetAllGeneralDepartmentsTest
    {
        private readonly Mock<IGeneralDepartmentSqlRepository> mockGeneralDepartmentRepository;
        private readonly GetAllGeneralDepartmentsHandler handler;

        public GetAllGeneralDepartmentsTest()
        {
            mockGeneralDepartmentRepository = new Mock<IGeneralDepartmentSqlRepository>();
            handler = new GetAllGeneralDepartmentsHandler(mockGeneralDepartmentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoGeneralDepartments()
        {
            // Arrange
            mockGeneralDepartmentRepository.Setup(r => r.FindAll(null, false)).Returns(new List<GeneralDepartment>().AsQueryable());
            var query = new GetAllGeneralDepartmentsRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllGeneralDepartments()
        {
            // Arrange
            var GeneralDepartments = new List<GeneralDepartment>
        {
            new() { Id = 1, DeCode = "D001", DeName = "HR" },
            new() { Id = 2, DeCode = "D002", DeName = "Finance" }
        };
            mockGeneralDepartmentRepository.Setup(repository => repository.FindAll(null, false)).Returns(GeneralDepartments.AsQueryable());
            var query = new GetAllGeneralDepartmentsRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, d => d.Id == 1 && d.DeName == "HR");
            Assert.Contains(result.Data, d => d.Id == 2 && d.DeName == "Finance");
        }      
    }
}
