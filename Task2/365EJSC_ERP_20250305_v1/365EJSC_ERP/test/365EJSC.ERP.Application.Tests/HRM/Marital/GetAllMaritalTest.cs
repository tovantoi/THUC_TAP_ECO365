using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.Marital;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.Marital
{
    public class GetAllMaritalTest
    {
        private readonly Mock<IMaritalSqlRepository> mockMaritalSqlRepository;
        private readonly GetAllMaritalHandler handler;

        public GetAllMaritalTest()
        {
            mockMaritalSqlRepository = new Mock<IMaritalSqlRepository>();
            handler = new GetAllMaritalHandler(mockMaritalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllSamples()
        {
            // Arrange
            var samples = new List<HrmMarital>
        {
            new() { Id = 1, Name = "Sample 1" },
            new() { Id = 2, Name = "Sample 2" }
        };
            mockMaritalSqlRepository
                .Setup(repository => repository.FindAll(null, false))
                .Returns(samples.AsQueryable());

            var query = new GetAllMaritalRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.Name == "Sample 1");
            Assert.Contains(result.Data, s => s.Id == 2 && s.Name == "Sample 2");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoSamples()
        {
            // Arrange
            mockMaritalSqlRepository
                .Setup(r => r.FindAll(null, false))
                .Returns(new List<HrmMarital>().AsQueryable());

            var query = new GetAllMaritalRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }

}
