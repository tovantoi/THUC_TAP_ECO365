using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyPosition
{
    public class DeleteCompanyPositionTest
    {
        private readonly Mock<ICompanyPositionSqlRepository> mockCompanyPositionSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteCompanyPositionHandler handler;

        public DeleteCompanyPositionTest()
        {
            mockCompanyPositionSqlRepository = new Mock<ICompanyPositionSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();

            handler = new DeleteCompanyPositionHandler(
                mockCompanyPositionSqlRepository.Object,
                mockSqlUnitOfWork.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteCompanyPositionRequest { Id = 1 };
            var companyPosition = new Entities.ErpGeneralCompanyPosition { Id = 1 };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyPosition);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanyPositionSqlRepository.Verify(repo => repo.Remove(It.IsAny<Entities.ErpGeneralCompanyPosition>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_CompanyPositionNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteCompanyPositionRequest { Id = 99 };

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.ErpGeneralCompanyPosition)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_POSITION_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteCompanyPositionRequest { Id = 1 };
            var companyPosition = new Entities.ErpGeneralCompanyPosition { Id = 1 };
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyPosition);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.Remove(It.IsAny<Entities.ErpGeneralCompanyPosition>()))
                .Throws(new Exception("Delete failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}