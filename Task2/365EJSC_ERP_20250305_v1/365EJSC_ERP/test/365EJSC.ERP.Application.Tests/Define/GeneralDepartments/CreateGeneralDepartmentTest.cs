using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments;
using _365EJSC.ERP.Application.Validators.Define.GeneralDepartments;
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
    /// Test class for creating GeneralDepartment entities.
    /// </summary>
    public class CreateGeneralDepartmentTest
    {
        private readonly Mock<IGeneralDepartmentSqlRepository> mockGeneralDepartmentSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateGeneralDepartmentHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGeneralDepartmentTest"/> class.
        /// </summary>
        public CreateGeneralDepartmentTest()
        {
            mockGeneralDepartmentSqlRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateGeneralDepartmentHandler(mockGeneralDepartmentSqlRepository.Object, mockSqlUnitOfWork.Object);
        }


        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateGeneralDepartmentRequest
            {
                DeCode = "test",
                DeName = "test",
                IsActived = true
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockGeneralDepartmentSqlRepository
                .Setup(repo => repo.Add(It.IsAny<GeneralDepartment>()))
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
            var request = new CreateGeneralDepartmentRequest
            {
                // Set invalid properties of CreateGeneralDepartmentRequest here
            };

            var validator = new CreateGeneralDepartmentValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_DEPARTMENT_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateGeneralDepartmentRequest
            {
                DeCode = "test",
                DeName = "test",
                IsActived = true
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockGeneralDepartmentSqlRepository
                .Setup(repo => repo.Add(It.IsAny<GeneralDepartment>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockGeneralDepartmentSqlRepository.Verify(repo => repo.Add(It.IsAny<GeneralDepartment>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }
    }
}
