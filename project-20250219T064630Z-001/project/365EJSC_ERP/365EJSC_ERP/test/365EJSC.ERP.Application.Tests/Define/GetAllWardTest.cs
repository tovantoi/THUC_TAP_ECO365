using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWard;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define
{
    public class GetAllWardTest
    {
        private readonly Mock<IWardSqlRepository> mockWardRepository;
        private readonly GetAllWardHandler handler;
        private readonly Mock<IDictrictSqlRepository> mockDictrictRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSampleTest"/> class.
        /// </summary>
        public GetAllWardTest()
        {
            mockWardRepository = new Mock<IWardSqlRepository>();
            handler = new GetAllWardHandler(mockWardRepository.Object, mockDictrictRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnAllSamples()
        {
            // Arrange
            var samples = new List<WebsiteLocalizationWard>
            {
                new() { Id = 1, Name = "Sample 1" },
                new() { Id = 2, Name = "Sample 2" }
            };
            mockWardRepository.Setup(repository => repository.FindAll(null, false)).Returns(samples.AsQueryable());
            var query = new GetAllWardQuery();

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
            mockWardRepository.Setup(r => r.FindAll(null, false)).Returns(new List<WebsiteLocalizationWard>().AsQueryable());
            var query = new GetAllWardQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
