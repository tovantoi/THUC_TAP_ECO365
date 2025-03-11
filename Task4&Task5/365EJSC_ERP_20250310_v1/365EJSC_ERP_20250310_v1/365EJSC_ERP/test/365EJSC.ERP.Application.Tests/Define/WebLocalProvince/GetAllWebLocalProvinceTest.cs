using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalProvince
{
    /// <summary>
    /// Test class for GetAllWebLocalProvinceHandler.
    /// </summary>
    public class GetAllWebLocalProvinceTest
    {
        private readonly Mock<IWebLocalProvinceSqlRepository> mockWebLocalProvinceSqlRepository;
        private readonly GetAllWebLocalProvinceHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllWebLocalProvinceTest"/> class.
        /// </summary>
        public GetAllWebLocalProvinceTest()
        {
            mockWebLocalProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            handler = new GetAllWebLocalProvinceHandler(mockWebLocalProvinceSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllWebLocalProvinces()
        {
            // Arrange
            var webLocalProvinces = new List<Domain.Entities.Define.WebLocalProvince>
            {
               new() { Id = 1, Name = "Sample 1"},
               new() { Id = 2, Name = "Sample 1"},
            };
            mockWebLocalProvinceSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(webLocalProvinces.AsQueryable());
            var query = new GetAllWebLocalProvinceRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoWebLocalProvince()
        {
            // Arrange
            mockWebLocalProvinceSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Domain.Entities.Define.WebLocalProvince>().AsQueryable());
            var query = new GetAllWebLocalProvinceRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
