using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.EmployeeRole
{
    public class GetAllEmployeeRoleTest
    {
        private readonly Mock<IEmployeeRoleSqlRepository> mockemployeeRoleSqlRepository;
        private readonly GetAllEmployeeRoleHandler handler;

        public GetAllEmployeeRoleTest()
        {
            mockemployeeRoleSqlRepository = new Mock<IEmployeeRoleSqlRepository>();
            handler = new GetAllEmployeeRoleHandler(mockemployeeRoleSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnAllSamples()
        {
            // Arrange
            var samples = new List<HrmEmployeeRole>
        {
            new() { Id = 1, Name = "Sample 1", Code = "NV01" },
            new() { Id = 2, Name = "Sample 2", Code = "NV02" }
        };
            mockemployeeRoleSqlRepository
                .Setup(repository => repository.FindAll(null, false))
                .Returns(samples.AsQueryable());

            var query = new GetAllEmployeeRoleRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.Name == "Sample 1" && s.Code == "NV01");
            Assert.Contains(result.Data, s => s.Id == 2 && s.Name == "Sample 2" && s.Code == "NV02");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoSamples()
        {
            // Arrange
            mockemployeeRoleSqlRepository
                .Setup(r => r.FindAll(null, false))
                .Returns(new List<HrmEmployeeRole>().AsQueryable());

            var query = new GetAllEmployeeRoleRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
