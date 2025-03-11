using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.UserCases.Product.ProductType;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Product.ProductType
{
    public class GetAllProductTypeTest
    {
        private readonly Mock<IProductTypeSqlRepository> mockProductTypeSqlRepository;
        private readonly GetAllProductTypeHandler handler;

        public GetAllProductTypeTest()
        {
            mockProductTypeSqlRepository = new Mock<IProductTypeSqlRepository>();
            handler = new GetAllProductTypeHandler(mockProductTypeSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnAllBanks()
        {
            // Arrange
            var samples = new List<PdProductType>
        {
            new() { Id = 1, Name = "Sample 1" },
            new() { Id = 2, Name = "Sample 2" }
        };
            mockProductTypeSqlRepository
                .Setup(repository => repository.FindAll(null, false))
                .Returns(samples.AsQueryable());

            var query = new GetAllProductTypeRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.Name == "Sample 1");
            Assert.Contains(result.Data, s => s.Id == 2 && s.Name == "Sample 2");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoBanks()
        {
            // Arrange
            mockProductTypeSqlRepository
                .Setup(r => r.FindAll(null, false))
                .Returns(new List<PdProductType>().AsQueryable());

            var query = new GetAllProductTypeRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
