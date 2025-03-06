using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWards;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalWards
{
    public class GetAllWebLocalWardTest
    {
        private readonly Mock<IWebLocalWardSqlRepository> mockWardRepository;
        private readonly GetAllWebLocalWardHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllWebLocalWardTest"/> class.
        /// </summary>
        public GetAllWebLocalWardTest()
        {
            mockWardRepository = new Mock<IWebLocalWardSqlRepository>();
            handler = new GetAllWebLocalWardHandler(mockWardRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllWards()
        {
            // Arrange
            var wards = new List<WebLocalWard>
            {
                new() { Id = 1, Name = "Ward 1", WebLocalDistrict = new WebLocalDistrict { WebLocalProvince = new WebLocalProvince() }},
                new() { Id = 2, Name = "Ward 2", WebLocalDistrict = new WebLocalDistrict { WebLocalProvince = new WebLocalProvince() }}
            };

            mockWardRepository
                    .Setup(repo => repo.FindAll(It.IsAny<Expression<Func<WebLocalWard, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WebLocalWard, object>>[]>()))
                    .Returns(() => wards.AsQueryable());



            var query = new GetAllWebLocalWardRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, w => w.Id == 1 && w.Name == "Ward 1");
            Assert.Contains(result.Data, w => w.Id == 2 && w.Name == "Ward 2");
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoWardsExist()
        {
            // Arrange
            var emptyList = new List<WebLocalWard>().AsQueryable();

            mockWardRepository
                .Setup(repo => repo.FindAll(null, false))
                .Returns(emptyList);

            var query = new GetAllWebLocalWardRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);  
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }


    }
}
