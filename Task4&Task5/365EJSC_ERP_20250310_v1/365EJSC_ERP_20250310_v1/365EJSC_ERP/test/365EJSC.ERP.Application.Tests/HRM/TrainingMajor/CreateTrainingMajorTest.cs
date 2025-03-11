using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor;
using _365EJSC.ERP.Application.Validators.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.TrainingMajor
{
    /// <summary>
    /// Test class for creating TrainingMajor entities.
    /// </summary>
    public class CreateTrainingMajorTest
    {
        private readonly Mock<ITrainingMajorSqlRepository> mockTrainingMajorSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateTrainingMajorHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTrainingMajorTest"/> class.
        /// </summary>
        public CreateTrainingMajorTest()
        {
            mockTrainingMajorSqlRepository = new Mock<ITrainingMajorSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateTrainingMajorHandler(mockTrainingMajorSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateTrainingMajorRequest
            {
            };

            var validator = new CreateTrainingMajorValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_TRAININGMAJOR_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateTrainingMajorRequest
            {
                TmName = "Test"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockTrainingMajorSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.HRM.TrainingMajor>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockTrainingMajorSqlRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.HRM.TrainingMajor>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateTrainingMajorRequest
            {
                TmName = "Test"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockTrainingMajorSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.HRM.TrainingMajor>()))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
  
    }
}
