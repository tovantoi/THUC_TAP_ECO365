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
    /// Test class for deleting TrainingMajor entities.
    /// </summary>
    public class DeleteTrainingMajorTest
    {
        private readonly Mock<ITrainingMajorSqlRepository> mockTrainingMajorSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteTrainingMajorHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTrainingMajorTest"/> class.
        /// </summary>
        public DeleteTrainingMajorTest()
        {
            mockTrainingMajorSqlRepository = new Mock<ITrainingMajorSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteTrainingMajorHandler(mockTrainingMajorSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteTrainingMajorRequest { Id = 1 };
            var TrainingMajor = new Domain.Entities.HRM.TrainingMajor
            {
                Id = 1
            };
            var mockTransaction = new Mock<IDbTransaction>();

            mockTrainingMajorSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TrainingMajor);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockTrainingMajorSqlRepository.Verify(repo => repo.Remove(TrainingMajor), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }


        [Fact]
        public async Task Handle_TrainingMajorNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteTrainingMajorRequest { Id = 1 };

            mockTrainingMajorSqlRepository
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
    }
}
