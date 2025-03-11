using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.UserCases.HRM.Bank;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.Bank
{
    public class DeleteBankTest
    {
        private readonly Mock<IBankSqlRepository> mockBankSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteBankHandler handler;

        public DeleteBankTest()
        {
            mockBankSqlRepository = new Mock<IBankSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteBankHandler(mockBankSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteBankRequest { Id = 1 };
            var sample = new HrmBank();
            var mockTransaction = new Mock<IDbTransaction>();

            mockBankSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id ?? 0, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockBankSqlRepository.Verify(repo => repo.Remove(sample), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_SampleNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteBankRequest { Id = 1 };

            mockBankSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id ?? 0, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_BANK_ID_NOT_FOUND
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_BANK_ID_NOT_FOUND, exception.MessageCode);

            mockSqlUnitOfWork.Verify(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteBankRequest { Id = 1 };
            var sample = new HrmBank();
            var mockTransaction = new Mock<IDbTransaction>();

            mockBankSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id ?? 0, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockBankSqlRepository
                .Setup(repo => repo.Remove(sample))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

            // Verify rollback được gọi
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}
