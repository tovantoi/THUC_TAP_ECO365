using System.Data;
using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.UserCases.Samples;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql.Base;
using _365Architect.Demo.Domain.Entities;
using Moq;

namespace _365Architect.Demo.Application.Tests.Command.Samples
{
    /// <summary>
    /// Test class for deleting Sample entities.
    /// </summary>
    public class DeleteSampleTest
    {
        private readonly Mock<ISampleSqlRepository> mockSampleSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteSampleHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSampleTest"/> class.
        /// </summary>
        public DeleteSampleTest()
        {
            mockSampleSqlRepository = new Mock<ISampleSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteSampleHandler(mockSampleSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteSampleCommand { Id = 1 };
            var sample = new Sample();
            var mockTransaction = new Mock<IDbTransaction>();

            mockSampleSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockSampleSqlRepository.Verify(repo => repo.Remove(sample), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_SampleNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteSampleCommand { Id = 1 };

            mockSampleSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_SAMPLE_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_SAMPLE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteSampleCommand { Id = 1 };
            var sample = new Sample();
            var mockTransaction = new Mock<IDbTransaction>();

            mockSampleSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSampleSqlRepository
                .Setup(repo => repo.Remove(sample))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}