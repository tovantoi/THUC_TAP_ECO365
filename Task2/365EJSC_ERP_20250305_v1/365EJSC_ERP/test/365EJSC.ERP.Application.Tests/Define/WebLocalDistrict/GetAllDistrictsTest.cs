using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalDistrict
{
    public class GetAllDistrictsTest
    {
        private readonly Mock<IWebLocalDistrictSqlRepository> mockDistrictRepository;
        private readonly Mock<IWebLocalProvinceSqlRepository> mockProvinceRepository;
        private readonly Mock<IWebLocalSqlRepository> mockLocalRepository;
        private readonly GetAllWebLocalDistrictHandler handler;

        public GetAllDistrictsTest()
        {
            mockDistrictRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockProvinceRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockLocalRepository = new Mock<IWebLocalSqlRepository>();
            handler = new GetAllWebLocalDistrictHandler(mockDistrictRepository.Object, mockProvinceRepository.Object, mockLocalRepository.Object);
        }

        [Fact]
        public async Task Handle_NoDistrictsFound_ReturnsEmptyList()
        {
            // Arrange
            var request = new GetAllWebLocalDistrictRequest(); // Empty query to get all districts

            // Mock the repository to return an empty list
            mockDistrictRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Entities.WebLocalDistrict>().AsQueryable());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data); // Ensure the list is empty
        }

        [Fact]
        public async Task Handle_DistrictsFound_ReturnsSuccessResult()
        {
            // Arrange
            var request = new GetAllWebLocalDistrictRequest(); // Empty query to get all districts

            // Mock the repository to return a list of districts
            var districtList = new List<Entities.WebLocalDistrict>
            {
                new Entities.WebLocalDistrict { Id = 1, Name = "District A" },
                new Entities.WebLocalDistrict { Id = 2, Name = "District B" }
            };
            mockDistrictRepository.Setup(repository => repository.FindAll(null, false)).Returns(districtList.AsQueryable());


            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count); // Ensure two districts are returned
            Assert.Contains(result.Data, d => d.Name == "District A");
            Assert.Contains(result.Data, d => d.Name == "District B");
        }
    }
}

