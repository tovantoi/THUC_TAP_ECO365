using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Define.GeneralDepartments
{
    /// <summary>
    /// Test class for deleting GeneralDepartment entities.
    /// </summary>
    public class DeleteGeneralDepartmentTest
    {
        private readonly Mock<IGeneralDepartmentSqlRepository> mockGeneralDepartmentSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteGeneralDepartmentHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteGeneralDepartmentTest"/> class.
        /// </summary>
        public DeleteGeneralDepartmentTest()
        {
            mockGeneralDepartmentSqlRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteGeneralDepartmentHandler(mockGeneralDepartmentSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteGeneralDepartmentRequest { Id = 1 };
            var generalDepartment = new GeneralDepartment
            {
                Id = 1,
                IsActived = true
            };
            var mockTransaction = new Mock<IDbTransaction>();

            mockGeneralDepartmentSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generalDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockGeneralDepartmentSqlRepository.Verify(repo => repo.Remove(generalDepartment), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }


        [Fact]
        public async Task Handle_GeneralDepartmentNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteGeneralDepartmentRequest { Id = 1 };

            mockGeneralDepartmentSqlRepository
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
            var request = new DeleteGeneralDepartmentRequest { Id = 1 };

            var generalDepartment = new GeneralDepartment();
            var mockTransaction = new Mock<IDbTransaction>();

            mockGeneralDepartmentSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generalDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockGeneralDepartmentSqlRepository
                .Setup(repo => repo.Remove(generalDepartment));            

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.INF_DELETED, exception.MessageCode);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

    }
}
