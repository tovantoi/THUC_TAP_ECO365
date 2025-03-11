using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.DefineContractType
{
    public class GetAllContractTypeTest
    {
        private readonly Mock<IContractTypeSqlRepository> mockContractTypeSqlRepository;
        private readonly GetAllContractTypeHandler handler;

        public GetAllContractTypeTest()
        {
            mockContractTypeSqlRepository = new Mock<IContractTypeSqlRepository>();
            handler = new GetAllContractTypeHandler(mockContractTypeSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoContractTypes()
        {
            // Arrange
            mockContractTypeSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Domain.Entities.HRM.DefineContractType>().AsQueryable());
            var query = new GetAllContractTypeRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllContractTypes()
        {
            // Arrange
            var defineContractTypes = new List<Domain.Entities.HRM.DefineContractType>
        {
            new() { Id = 1, Code = "D001", Name = "HR" },
            new() { Id = 2, Code = "D002", Name = "Finance" }
        };
            mockContractTypeSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(defineContractTypes.AsQueryable());
            var query = new GetAllContractTypeRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, d => d.Id == 1 && d.Name == "HR");
            Assert.Contains(result.Data, d => d.Id == 2 && d.Name == "Finance");
        }
    }
}
