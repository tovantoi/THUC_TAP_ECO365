using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWards;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalWards
{
    public class GetAllWebLocalWardTest
    {
        private readonly Mock<IWebLocalWardSqlRepository> mockWardRepository;
        private readonly GetAllWebLocalWardHandler handler;
        private readonly Mock<IWebLocalDictrictSqlRepository> mockDictrictRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSampleTest"/> class.
        /// </summary>
        public GetAllWebLocalWardTest()
        {
            mockWardRepository = new Mock<IWebLocalWardSqlRepository>();
            handler = new GetAllWebLocalWardHandler(mockWardRepository.Object, mockDictrictRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnAllSamples()
        {
            // Arrange
            var samples = new List<Domain.Entities.Define.WebLocalWard>
            {
                new() { Id = 1, Name = "Sample 1" },
                new() { Id = 2, Name = "Sample 2" }
            };
            mockWardRepository.Setup(repository => repository.FindAll(null, false)).Returns(samples.AsQueryable());
            var query = new GetAllWebLocalWardRequest();

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
            mockWardRepository.Setup<IQueryable<WebLocalWard>>(r => r.FindAll(null, false)).Returns(new List<WebLocalWard>().AsQueryable());
            var query = new GetAllWebLocalWardRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
