using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.UserCases.Samples;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Entities;
using Moq;

namespace _365Architect.Demo.Application.Tests.Query.Samples
{
    /// <summary>
    /// Test class for GetAllSampleQueryHandler.
    /// </summary>
    public class GetAllSampleTest
    {
        private readonly Mock<ISampleSqlRepository> mockSampleRepository;
        private readonly GetAllSampleHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSampleTest"/> class.
        /// </summary>
        public GetAllSampleTest()
        {
            mockSampleRepository = new Mock<ISampleSqlRepository>();
            handler = new GetAllSampleHandler(mockSampleRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllSamples()
        {
            // Arrange
            var samples = new List<Sample>
            {
                new() { Id = 1, Title = "Sample 1" },
                new() { Id = 2, Title = "Sample 2" }
            };
            mockSampleRepository.Setup(repository => repository.FindAll(null, false)).Returns(samples.AsQueryable());
            var query = new GetAllSampleQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.Title == "Sample 1");
            Assert.Contains(result.Data, s => s.Id == 2 && s.Title == "Sample 2");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoSamples()
        {
            // Arrange
            mockSampleRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Sample>().AsQueryable());
            var query = new GetAllSampleQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}