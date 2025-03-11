using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.UserCases.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocal
{
    public class CreateWebLocalTest
    {
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateWebLocalHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateWebLocalTest"/> class.
        /// </summary>
        public CreateWebLocalTest()
        {
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateWebLocalHandler(mockWebLocalSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateWebLocalRequest
            {
                Id = "AAA",
                IsActived = true,
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalSqlRepository
                .Setup(repo => repo.Add(It.IsAny<WebLocals>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockWebLocalSqlRepository.Verify(repo => repo.Add(It.IsAny<WebLocals>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateWebLocalRequest
            {
                Id = "AAA",
                IsActived = true,
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalSqlRepository
                .Setup(repo => repo.Add(It.IsAny<WebLocals>()))
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
            var request = new CreateWebLocalRequest
            {
                // Set invalid properties of CreateWebLocalCommand here
            };

            var validator = new CreateWebLocalValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_LOCAL_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}