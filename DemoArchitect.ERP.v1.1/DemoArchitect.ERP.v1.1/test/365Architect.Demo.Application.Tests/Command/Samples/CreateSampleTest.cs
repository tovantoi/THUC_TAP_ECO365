using System.Data;
using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.UserCases.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;
using Moq;

namespace _365Architect.Demo.Application.Tests.Command.Samples
{
    /// <summary>
    /// Test class for creating Sample entities.
    /// </summary>
    public class CreateSampleTest
    {
        private readonly Mock<ISampleSqlRepository> mockSampleSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateSampleHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSampleTest"/> class.
        /// </summary>
        public CreateSampleTest()
        {
            mockSampleSqlRepository = new Mock<ISampleSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateSampleHandler(mockSampleSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateSampleCommand
            {
                Title = "test",
                DueDate = DateTime.Today,
                Description = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSampleSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Sample>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockSampleSqlRepository.Verify(repo => repo.Add(It.IsAny<Sample>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateSampleCommand
            {
                Title = "test",
                DueDate = DateTime.Today,
                Description = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSampleSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Sample>()))
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
            var request = new CreateSampleCommand
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new CreateSampleValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_SAMPLE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}