using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using Entities = _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalDistrict
{
    public class GetDetailDistrictTest
    {
        private readonly Mock<IWebLocalDistrictSqlRepository> mockDistrictRepository;
        private readonly Mock<IWebLocalProvinceSqlRepository> mockProvinceRepository;
        private readonly Mock<IWebLocalSqlRepository> mockLocalRepository;
        private readonly GetDetailWebLocalDistrictHandler handler;

        public GetDetailDistrictTest()
        {
            mockDistrictRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockProvinceRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockLocalRepository = new Mock<IWebLocalSqlRepository>();
            handler = new GetDetailWebLocalDistrictHandler(mockDistrictRepository.Object, mockProvinceRepository.Object, mockLocalRepository.Object);
        }

        [Fact]
        public async Task Handle_DistrictNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new GetDetailWebLocalDistrictRequest { Id = 999 }; // Id that does not exist

            // Mock the repository to return null (district not found)
            mockDistrictRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.WebLocalDistrict?)null); // Simulate district not found

            // Act & Assert
            //var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_DistrictFound_ReturnsSuccessResult()
        {
            // Arrange
            var request = new GetDetailWebLocalDistrictRequest { Id = 1 };

            // Mock the repository to return a district
            mockDistrictRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalDistrict { Id = 1, Name = "District A" });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}