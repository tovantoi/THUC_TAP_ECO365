using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.Bank;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.Bank
{
    public class GetAllBankTest
    {
        private readonly Mock<IBankSqlRepository> mockBankSqlRepository;
        private readonly GetAllBankHandler handler;

        public GetAllBankTest()
        {
            mockBankSqlRepository = new Mock<IBankSqlRepository>();
            handler = new GetAllBankHandler(mockBankSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnAllBanks()
        {
            // Arrange
            var samples = new List<HrmBank>
        {
            new() { Id = 1, Name = "Sample 1" },
            new() { Id = 2, Name = "Sample 2" }
        };
            mockBankSqlRepository
                .Setup(repository => repository.FindAll(null, false))
                .Returns(samples.AsQueryable());

            var query = new GetAllBankRequest();

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
            mockBankSqlRepository
                .Setup(r => r.FindAll(null, false))
                .Returns(new List<HrmBank>().AsQueryable());

            var query = new GetAllBankRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
