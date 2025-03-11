using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyPosition
{
    public class GetAllCompanyPositionTest
    {
        private readonly Mock<ICompanyPositionSqlRepository> mockCompanyPositionRepository;
        private readonly GetAllCompanyPositionHandler handler;

        public GetAllCompanyPositionTest()
        {
            mockCompanyPositionRepository = new Mock<ICompanyPositionSqlRepository>();
            handler = new GetAllCompanyPositionHandler(mockCompanyPositionRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllCompanyPositions()
        {
            // Arrange
            var companyPositions = new List<Entities.ErpGeneralCompanyPosition>
            {
                new() { Id = 1, CompanyId = 1, PositionId = 1 },
                new() { Id = 2, CompanyId = 2, PositionId = 2 }
            };

            mockCompanyPositionRepository.Setup(repository => repository.FindAll(null, false)).Returns(companyPositions.AsQueryable());

            var query = new GetAllCompanyPositionRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Id == 1 && s.CompanyId == 1 && s.PositionId == 1);
            Assert.Contains(result.Data, s => s.Id == 2 && s.CompanyId == 2 && s.PositionId == 2);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoCompanyPositions()
        {
            // Arrange
            mockCompanyPositionRepository.Setup(repository => repository.FindAll(null, false)).Returns(new List<Entities.ErpGeneralCompanyPosition>().AsQueryable());

            var query = new GetAllCompanyPositionRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}