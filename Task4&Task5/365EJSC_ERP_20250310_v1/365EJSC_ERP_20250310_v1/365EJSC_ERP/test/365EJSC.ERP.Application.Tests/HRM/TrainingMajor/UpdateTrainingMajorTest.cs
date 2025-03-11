using _365EJSC.ERP.Application.Requests.HRM;
using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.UserCases.HRM;
using _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.TrainingMajor
{
    /// <summary>
    /// Test class for updating TrainingMajor entities.
    /// </summary>
    public class UpdateTrainingMajorTest
    {
        private readonly Mock<ITrainingMajorSqlRepository> mockTrainingMajorRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateTrainingMajorHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTrainingMajorTest"/> class.
        /// </summary>
        public UpdateTrainingMajorTest()
        {
            mockTrainingMajorRepository = new Mock<ITrainingMajorSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateTrainingMajorHandler(mockTrainingMajorRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateTrainingMajorRequest { Id = 1, TmName = "test" };
            var TrainingMajor = new Domain.Entities.HRM.TrainingMajor();
            var mockTransaction = new Mock<IDbTransaction>();

            mockTrainingMajorRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TrainingMajor);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockTrainingMajorRepository.Verify(repo => repo.Update(TrainingMajor), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_TrainingMajorNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateTrainingMajorRequest { Id = 1 };

            mockTrainingMajorRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateTrainingMajorRequest { Id = 1 };
            var TrainingMajor = new Domain.Entities.HRM.TrainingMajor();
            var mockTransaction = new Mock<IDbTransaction>();

            mockTrainingMajorRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TrainingMajor);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockTrainingMajorRepository
                .Setup(repo => repo.Update(TrainingMajor))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}
