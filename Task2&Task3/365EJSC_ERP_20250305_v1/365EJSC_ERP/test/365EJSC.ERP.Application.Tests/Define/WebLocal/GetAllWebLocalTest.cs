using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.UserCases.Define.WebLocal;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocal
{
    public class GetAllWebLocalTest
    {
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly GetAllWebLocalHandler handler;

        public GetAllWebLocalTest()
        {
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            handler = new GetAllWebLocalHandler(mockWebLocalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllWebLocals()
        {
            // Arrange
            var webLocals = new List<WebLocals>
            {
                new() { Id = "AAA", Localization = "Local 1", IsActived = true },
                new() { Id = "BBB", Localization = "Local 2", IsActived = true }
            };

            mockWebLocalSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(webLocals.AsQueryable());
            var query = new GetAllWebLocalRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, l => l.Id == "AAA" && l.Localization == "Local 1");
            Assert.Contains(result.Data, l => l.Id == "BBB" && l.Localization == "Local 2");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoWebLocals()
        {
            // Arrange
            mockWebLocalSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<WebLocals>().AsQueryable());

            var query = new GetAllWebLocalRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}