using _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;
using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;

namespace _365EJSC.ERP.Application.Tests.Define.GeneralDepartments
{
    /// <summary>
    /// Test class for updating GeneralDepartment entities.
    /// </summary>
    public class UpdateGeneralDepartmentTest
    {
        private readonly Mock<IGeneralDepartmentSqlRepository> mockGeneralDepartmentRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateGeneralDepartmentHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGeneralDepartmentTest"/> class.
        /// </summary>
        public UpdateGeneralDepartmentTest()
        {
            mockGeneralDepartmentRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateGeneralDepartmentHandler(mockGeneralDepartmentRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateGeneralDepartmentRequest { Id = 1, DeName = "HR" };
            var generalDepartment = new GeneralDepartment();
            var mockTransaction = new Mock<IDbTransaction>();

            mockGeneralDepartmentRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generalDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockGeneralDepartmentRepository.Verify(repo => repo.Update(generalDepartment), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_GeneralDepartmentNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateGeneralDepartmentRequest { Id = 1 };

            mockGeneralDepartmentRepository
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
            var request = new UpdateGeneralDepartmentRequest { Id = 1 };
            var generalDepartment = new GeneralDepartment();
            var mockTransaction = new Mock<IDbTransaction>();

            mockGeneralDepartmentRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generalDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockGeneralDepartmentRepository
                .Setup(repo => repo.Update(generalDepartment))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}
