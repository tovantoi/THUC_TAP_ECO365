using _365EJSC.ERP.Application.UserCases.Define.WebLocalWard;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using System.Data;
using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define
{ 
    public class DeleteWardTest
    {
        private readonly Mock<IWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteWardHandler handler;

        public DeleteWardTest()
        {
            mockWardSqlRepository = new Mock<IWardSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteWardHandler(mockWardSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteWardCommand { Id = 1 };
            var sample = new WebsiteLocalizationWard();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockWardSqlRepository.Verify(repo => repo.Remove(sample), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_SampleNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteWardCommand { Id = 1 };

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_WARD_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteWardCommand { Id = 1 };
            var sample = new WebsiteLocalizationWard();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWardSqlRepository
                .Setup(repo => repo.Remove(sample))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }

}
