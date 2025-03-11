using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalDistrict
{
    public class DeleteDistrictTest
    {
        private readonly Mock<IWebLocalDistrictSqlRepository> mockDistrictSqlRepository;
        private readonly Mock<IWebLocalWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteWebLocalDistrictHandler handler;

        public DeleteDistrictTest()
        {
            mockDistrictSqlRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockWardSqlRepository = new Mock<IWebLocalWardSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteWebLocalDistrictHandler(mockDistrictSqlRepository.Object, mockWardSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteWebLocalDistrictRequest { Id = 1 };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockDistrictSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalDistrict { Id = 1 }); // Simulate finding the district

            mockDistrictSqlRepository
                .Setup(repo => repo.Remove(It.IsAny<Entities.WebLocalDistrict>())); // Simulate removing the district

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate successful save

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockDistrictSqlRepository.Verify(repo => repo.Remove(It.IsAny<Entities.WebLocalDistrict>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }
    }
}
