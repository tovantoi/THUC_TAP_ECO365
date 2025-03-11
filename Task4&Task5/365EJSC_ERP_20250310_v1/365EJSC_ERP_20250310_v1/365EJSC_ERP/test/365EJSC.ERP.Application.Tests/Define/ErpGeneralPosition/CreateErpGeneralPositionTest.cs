using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralPosition
{
    public class CreateErpGeneralPositionTest
    {
        private readonly Mock<IErpGeneralPositionSqlRepository> mockErpGeneralPositionSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateErpGeneralPositionHandler handler;

        public CreateErpGeneralPositionTest()
        {
            mockErpGeneralPositionSqlRepository = new Mock<IErpGeneralPositionSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateErpGeneralPositionHandler(mockErpGeneralPositionSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateErpGeneralPositionRequest
            {
                Code = "TP01",
                Name = "Trưởng Phòng Nhân sự",
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockErpGeneralPositionSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.Define.ErpGeneralPosition>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockErpGeneralPositionSqlRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Define.ErpGeneralPosition>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateErpGeneralPositionRequest
            {
                Code = "PT01",
                Name = "Trưởng Phòng Tài chính",
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockErpGeneralPositionSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.Define.ErpGeneralPosition>()))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateErpGeneralPositionRequest
            {
                Code = "PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01PT01",
                //Name = "Test",

            };

            var validator = new CreateErpGeneralPositionValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_POSITION_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
