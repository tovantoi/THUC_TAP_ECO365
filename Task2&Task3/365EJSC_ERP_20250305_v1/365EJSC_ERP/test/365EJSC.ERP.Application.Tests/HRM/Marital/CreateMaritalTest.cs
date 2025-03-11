using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.Marital
{
    public class CreateMaritalTest
    {
        private readonly Mock<IMaritalSqlRepository> mockMaritalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateMaritalHandler handler;

        public CreateMaritalTest()
        {
            mockMaritalSqlRepository = new Mock<IMaritalSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateMaritalHandler(mockMaritalSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateMaritalRequest
            {
                Name = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockMaritalSqlRepository
                .Setup(repo => repo.Add(It.IsAny<HrmMarital>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockMaritalSqlRepository.Verify(repo => repo.Add(It.IsAny<HrmMarital>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateMaritalRequest
            {
                Name = "test",
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockMaritalSqlRepository
                .Setup(repo => repo.Add(It.IsAny<HrmMarital>()))
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
            var request = new CreateMaritalRequest
            {
                // Set invalid properties of CreateMaritalRequest here
            };

            var validator = new CreateMaritalValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_MARITAL_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
