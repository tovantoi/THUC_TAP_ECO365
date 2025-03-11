using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.UserCases.Define.WebLocal;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocal
{
    public class UpdateWebLocalTest
    {
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateWebLocalHandler handler;

        public UpdateWebLocalTest()
        {
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateWebLocalHandler(mockWebLocalSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateWebLocalRequest
            {
                Id = "AAA"
            };

            var webLocal = new WebLocals();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocal);

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
            mockWebLocalSqlRepository.Verify(repo => repo.Update(webLocal), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WebLocalNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateWebLocalRequest { Id = "AAA" };

            mockWebLocalSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((WebLocals)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_KEY_LOCAL_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateWebLocalRequest { Id = "AAA" };
            var webLocal = new WebLocals();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalSqlRepository
                .Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocal);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalSqlRepository
                .Setup(repo => repo.Update(webLocal))
                .Throws(new Exception("Update failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}
