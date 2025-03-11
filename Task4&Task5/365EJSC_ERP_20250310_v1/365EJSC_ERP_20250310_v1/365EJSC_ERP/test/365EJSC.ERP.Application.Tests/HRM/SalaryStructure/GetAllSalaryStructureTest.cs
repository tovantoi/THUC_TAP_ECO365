using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.SalaryStructure
{
    public class GetAllSalaryStructureTest
    {
        private readonly Mock<ISalaryStructureSqlRepository> mockSalaryStructureSqlRepository;
        private readonly GetAllSalaryStructureHandler handler;

        public GetAllSalaryStructureTest()
        {
            mockSalaryStructureSqlRepository = new Mock<ISalaryStructureSqlRepository>();
            handler = new GetAllSalaryStructureHandler(mockSalaryStructureSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllSalaryStructures()
        {
            // Arrange
            var erpGeneralPositions = new List<Domain.Entities.HRM.DefineSalaryStructure>
            {
               new() { Id = 1, Code = "", Name = "Test 1"},
               new() { Id = 2, Code = "", Name = "Test 2"},
            };
            mockSalaryStructureSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(erpGeneralPositions.AsQueryable());
            var query = new GetAllSalaryStructureRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoSalaryStructure()
        {
            // Arrange
            mockSalaryStructureSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Domain.Entities.HRM.DefineSalaryStructure>().AsQueryable());
            var query = new GetAllSalaryStructureRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}